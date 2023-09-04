using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Exceptions.AuthException;
using Bookory.Business.Utilities.Exceptions.BasketException;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Business.Utilities.Exceptions.LoginException;
using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class BasketService : IBasketService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBookService _bookService;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IShoppingSessionRepository _shoppingSessionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly bool _isAuthenticated;
    private const string COOKIE_BASKET_ITEM_KEY = "mybasketitemkey";

    public BasketService(UserManager<AppUser> userManager, IBasketItemRepository basketItemRepository, IShoppingSessionRepository shoppingSessionRepository, IHttpContextAccessor httpContextAccessor, IBookRepository bookRepository, IMapper mapper, IBookService bookService)
    {
        _userManager = userManager;
        _basketItemRepository = basketItemRepository;
        _shoppingSessionRepository = shoppingSessionRepository;
        _httpContextAccessor = httpContextAccessor;
        _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _bookService = bookService;
    }

    public async Task<ResponseDto> AddItemToBasketAsync(BasketPostDto basketPostDto)
    {
        ValidateInputPost(basketPostDto);
        var book = await GetBookByIdAsync(basketPostDto.Id);

        if (!_isAuthenticated)
        {
            ResponseDto response = await AddBasketItemToCookieAsync(book, basketPostDto);
            return new ResponseDto(response.StatusCode, response.Message);
        }


        var userId = await GetUserIdAsync();

        var userSession = await GetOrCreateUserSessionAsync(userId);
        var existingBasketItem = await GetExistingBasketItemAsync(userSession, book);

        if (existingBasketItem is null)
        {
            BasketItem basketItem = new BasketItem
            {
                BookId = book.Id,
                Price = book.Price,
                Quantity = basketPostDto.Quantity,
                //SessionId = userSession.Id, //Session Id while await _shoppingSessionRepository.SaveAsync();
            };
            userSession.BasketItems.Add(basketItem);
            userSession.UserId = userId;
        }
        else
        {
            existingBasketItem.Quantity += basketPostDto.Quantity;
        }

        userSession.TotalPrice = userSession.BasketItems.Sum(p => p.Quantity * p.Price);

        if (!await _shoppingSessionRepository.IsExistAsync(ss => ss.Id == userSession.Id))
            await _shoppingSessionRepository.CreateAsync(userSession);
        await _shoppingSessionRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.Created, "Book successfully added");
    }

    public async Task<List<BasketGetResponseDto>> GetActiveUserBasket()
    {
        if (!_isAuthenticated)
        {
            var cookieBasketItems = GetBasketItemsFromCookie();
            await IncludeBookToBasketItemAsync(cookieBasketItems);

            return _mapper.Map<List<BasketGetResponseDto>>(cookieBasketItems);
        }

        var userId = await GetUserIdAsync();
        ShoppingSession userSession = await GetUserSessionWithIncludesAsync(userId);

        var basketItems = _mapper.Map<List<BasketGetResponseDto>>(userSession.BasketItems);

        return basketItems;
    }

    public async Task<List<BasketGetResponseDto>> GetBasketItemAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
            throw new UserNotFoundException($"User not found by Id {id}");

        ShoppingSession userSession = await GetUserSessionWithIncludesAsync(user.Id);
        var basketItems = _mapper.Map<List<BasketGetResponseDto>>(userSession.BasketItems);

        return basketItems;
    }

    public async Task<ResponseDto> UpdateItemAsync(BasketPutDto basketPutDto)
    {
        ValidateInputPut(basketPutDto);
        var book = await GetBookByIdAsync(basketPutDto.Id);

        if (!_isAuthenticated)
        {
            var response = await UpdateCookieBasketItemAsync(book, basketPutDto);
            return new ResponseDto(response.StatusCode, response.Message);
        }

        var userId = await GetUserIdAsync();
        var userSession = await GetUserSessionWithIncludesAsync(userId);

        var existingBasketItem = await GetExistingBasketItemAsync(userSession, book);

        if (existingBasketItem is null)
            throw new BasketItemNotFoundException("The specified item does not exist in the basket");

        existingBasketItem.Quantity = basketPutDto.Quantity;

        userSession.TotalPrice = userSession.BasketItems.Sum(p => p.Quantity * p.Price);

        _basketItemRepository.Update(existingBasketItem);
        await _basketItemRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "Item quantity in the basket has been successfully updated");
    }

    public async Task<ResponseDto> RemoveBasketItemAsync(Guid id)
    {
        bool isExist = await _bookRepository.IsExistAsync(b => b.Id == id);
        if (!isExist)
            throw new BookNotFoundException("Book does not exist in basket");

        if (!_isAuthenticated)
        {
            var response = await RemoveCookieBasketItemAsync(id);
            return new ResponseDto(response.StatusCode, response.Message);
        }

        var userId = await GetUserIdAsync();
        ShoppingSession? userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered);

        var book = await _basketItemRepository.GetSingleAsync(bi => bi.BookId == id);
        if (book is null)
            throw new BookNotFoundException($"Book does not exist in basket");

        _basketItemRepository.Delete(book);
        await _basketItemRepository.SaveAsync();
        return new ResponseDto((int)HttpStatusCode.OK, "Product is deleted");
    }

    public async Task<ResponseDto> TransferCookieBasketToDatabaseAsync(string userId)
    {
        var cookieBasketItems = GetBasketItemsFromCookie();
        if (cookieBasketItems == null || !cookieBasketItems.Any())
            return new ResponseDto((int)HttpStatusCode.OK, "No cookie basket items to transfer");

        ShoppingSession userSession = await GetOrCreateUserSessionAsync(userId);

        foreach (var basketItem in cookieBasketItems)
        {
            var existingBasketItem = userSession.BasketItems.FirstOrDefault(bi => bi.BookId == basketItem.BookId);

            if (existingBasketItem != null)
                existingBasketItem.Quantity += basketItem.Quantity;
            else
                userSession.BasketItems.Add(basketItem);
        }

        userSession.TotalPrice = userSession.BasketItems.Sum(bi => bi.Quantity * bi.Price);
        _shoppingSessionRepository.Update(userSession);
        await _shoppingSessionRepository.SaveAsync();

        ClearCookieBasket();

        return new ResponseDto((int)HttpStatusCode.OK, "Cookie basket successfully transferred to the database.");
    }

    #region Cookie Mehtods
    private List<BasketItem> GetBasketItemsFromCookie()
    {
        List<BasketItem> basketItems = null;
        var cookie = _httpContextAccessor.HttpContext.Request.Cookies[COOKIE_BASKET_ITEM_KEY];
        if (cookie != null)
            basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(cookie);
        return basketItems;
    }
    public async Task<ResponseDto> AddBasketItemToCookieAsync(Book book, BasketPostDto basketPostDto)
    {
        List<BasketItem> basketItems = GetBasketItemsFromCookie();
        BasketItem basketItem = new BasketItem
        {
            BookId = book.Id,
            Price = book.Price,
            Quantity = basketPostDto.Quantity,
        };

        if (basketItems != null)
            if (basketItems.Any(bi => bi.BookId == book.Id))
                basketItems.FirstOrDefault(bi => bi.BookId == book.Id).Quantity += basketPostDto.Quantity;
            else
                basketItems.Add(basketItem);
        else
            basketItems = new List<BasketItem> { basketItem };

        _httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_BASKET_ITEM_KEY, JsonConvert.SerializeObject(basketItems));

        return new ResponseDto((int)HttpStatusCode.Created, "Book successfully added");
    }
    private async Task<ResponseDto> UpdateCookieBasketItemAsync(Book book, BasketPutDto basketPutDto)
    {
        List<BasketItem> basketItems = GetBasketItemsFromCookie();
        if (basketItems is null)
            throw new BasketEmptyException("Your basket is empty");

        BasketItem itemToUpdate = basketItems.FirstOrDefault(bi => bi.BookId == basketPutDto.Id);
        if (itemToUpdate is null)
            throw new BasketItemNotFoundException("The specified item does not exist in the basket");

        itemToUpdate.Quantity = basketPutDto.Quantity;

        _httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_BASKET_ITEM_KEY, JsonConvert.SerializeObject(basketItems));

        return new ResponseDto((int)HttpStatusCode.OK, "Item quantity in the basket has been successfully updated");
    }
    private async Task<ResponseDto> RemoveCookieBasketItemAsync(Guid id)
    {
        var basketItems = GetBasketItemsFromCookie();

        if (basketItems is null)
            throw new BasketEmptyException("Your basket is empty");

        BasketItem itemToDelete = basketItems.FirstOrDefault(bi => bi.BookId == id);
        if (itemToDelete is null)
            throw new BasketItemNotFoundException("The specified item does not exist in the basket");

        basketItems.Remove(itemToDelete);

        _httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_BASKET_ITEM_KEY, JsonConvert.SerializeObject(basketItems));
        return new ResponseDto((int)HttpStatusCode.OK, "Item removed from the basket");
    }
    private void ClearCookieBasket()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(COOKIE_BASKET_ITEM_KEY);
    }

    #endregion

    #region AddItemToBasketAsync Methods

    private async Task<ShoppingSession> GetOrCreateUserSessionAsync(string userId)
    {
        var userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered, nameof(ShoppingSession.BasketItems));

        return userSession ?? new ShoppingSession { UserId = userId };
    }

    private async Task<BasketItem> GetExistingBasketItemAsync(ShoppingSession userSession, Book book)
    {
        return await _basketItemRepository.GetSingleAsync(ss => ss.SessionId == userSession.Id && ss.BookId == book.Id);
    }

    private async Task<Book> GetBookByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book is null)
            throw new BookNotFoundException($"Book with ID {id} was not found");
        return book;
    }

    private static void ValidateInputPut(BasketPutDto basketPutDto)
    {
        if (basketPutDto.Quantity <= 0)
            throw new InvalidQuantityException("Invalid Quantity");

        if (basketPutDto is null)
            throw new InvalidInputDataException("Invalid input data");
    }

    private static void ValidateInputPost(BasketPostDto basketPostDto)
    {
        if (basketPostDto.Quantity <= 0)
            throw new InvalidQuantityException("Invalid Quantity");

        if (basketPostDto is null)
            throw new InvalidInputDataException("Invalid input data");
    }


    #endregion

    #region Common Methods
    private async Task<string> GetUserIdAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new UserNotFoundException($"User can not find by");
        return userId;
    }

    private async Task IncludeBookToBasketItemAsync(List<BasketItem> cookieBasketItems)
    {
        if (cookieBasketItems != null)
        {
            foreach (var cookieItem in cookieBasketItems)
            {
                cookieItem.Book = await _bookRepository.GetSingleAsync(b => b.Id == cookieItem.BookId,
                                                                                    nameof(Book.Images),
                                                                                    nameof(Book.Author),
                                                                                    $"{nameof(Book.Author)}.{nameof(Author.Images)}",
                                                                                    $"{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}");
            }
        }
    }

    private async Task<ShoppingSession> GetUserSessionWithIncludesAsync(string userId)
    {
        ShoppingSession? userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered,
        nameof(ShoppingSession.BasketItems), // Include BasketItems
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book), // Include Book
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Images), // Include Book Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Author) + "." + nameof(Author.Images), // Include Author Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.BookGenres) + "." + nameof(BookGenre.Genre)) ?? new ShoppingSession();
        return userSession;
    }

    #endregion
}
