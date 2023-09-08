using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Exceptions.AuthException;
using Bookory.Business.Utilities.Exceptions.BasketException;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class BasketService : IBasketService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    private readonly bool _isAuthenticated;
    private const string COOKIE_BASKET_ITEM_KEY = "mybasketitemkey";

    private readonly IShoppingSessionService _shoppingSessionService;
    private readonly IBasketItemService _basketItemService;
    private readonly IUserService _userService;

    private readonly IShoppingSessionRepository _shoppingSessionRepository;

    private readonly IBookRepository _bookRepository; // 1 dene qaldi
    public BasketService(IShoppingSessionRepository shoppingSessionRepository, IHttpContextAccessor httpContextAccessor, IBookRepository bookRepository, IMapper mapper, IBookService bookService, IShoppingSessionService shoppingSessionService, IUserService userService, IBasketItemService basketItemService)
    {
        _shoppingSessionRepository = shoppingSessionRepository;
        _httpContextAccessor = httpContextAccessor;
        _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        _bookRepository = bookRepository;
        _mapper = mapper;
        _bookService = bookService;
        _shoppingSessionService = shoppingSessionService;
        _userService = userService;
        _basketItemService = basketItemService;
    }

    public async Task<ResponseDto> AddItemToBasketAsync(BasketPostDto basketPostDto) // Don't use _shoppingSessionRepository update logic 
    {
        var book = await _bookService.GetBookByIdAsync(basketPostDto.Id);

        if (!_isAuthenticated)
        {
            ResponseDto response = await AddBasketItemToCookieAsync(book, basketPostDto);
            return new ResponseDto(response.StatusCode, response.Message);
        }
        var userId = await GetUserIdAsync();

        var userSession = await GetOrCreateUserSessionAsync(userId);
        var existingBasketItem = await _basketItemService.GetExistingBasketItemAsync(userSession.Id, book.Id);

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

        if (!await _shoppingSessionService.ShoppingSessionIsExistAsync(userSession.Id))
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

    public async Task<List<BasketGetResponseDto>> GetBasketItemAsync(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user is null)
            throw new UserNotFoundException($"User not found by Id {id}");

        ShoppingSession userSession = await GetUserSessionWithIncludesAsync(user.User.Id.ToString());
        var basketItems = _mapper.Map<List<BasketGetResponseDto>>(userSession.BasketItems);

        return basketItems;
    }

    public async Task<ResponseDto> UpdateItemAsync(BasketPutDto basketPutDto)
    {
        var book = await _bookService.GetBookByIdAsync(basketPutDto.Id);

        if (!_isAuthenticated)
        {
            var response = await UpdateCookieBasketItemAsync(basketPutDto);
            return new ResponseDto(response.StatusCode, response.Message);
        }

        var userId = await GetUserIdAsync();
        var userSession = await GetUserSessionWithIncludesAsync(userId);

        var existingBasketItem = await _basketItemService.GetExistingBasketItemAsync(userSession.Id, book.Id);

        if (existingBasketItem is null)
            throw new BasketItemNotFoundException("The specified item does not exist in the basket");

        existingBasketItem.Quantity = basketPutDto.Quantity;

        userSession.TotalPrice = userSession.BasketItems.Sum(p => p.Quantity * p.Price);

        await _basketItemService.UpdateBasketItemAsync(existingBasketItem);

        return new ResponseDto((int)HttpStatusCode.OK, "Item quantity in the basket has been successfully updated");
    }

    public async Task<ResponseDto> RemoveBasketItemAsync(Guid id)
    {
        bool isExist = await _bookService.BookIsExistAsync(id);
        if (!isExist)
            throw new BookNotFoundException("Book does not exist in basket");

        if (!_isAuthenticated)
        {
            var res = await RemoveCookieBasketItemAsync(id);
            return new ResponseDto(res.StatusCode, res.Message);
        }

        var userId = await GetUserIdAsync();
        ShoppingSession? userSession = await _shoppingSessionService.GetShoppingSessionByUserIdAsync(userId);

        var basketBook = await _basketItemService.GetBasketItemByBookIdAsync(id);

        var response = await _basketItemService.DeleteBasketItemAsync(basketBook.Id);
        return new ResponseDto(response.StatusCode, response.Message);
    } //OK

    public async Task<ResponseDto> TransferCookieBasketToDatabaseAsync(string userId) //OK
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

        await _shoppingSessionService.UpdateShoppingSessionAsync(userSession);

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
    public async Task<ResponseDto> AddBasketItemToCookieAsync(BookGetResponseDto book, BasketPostDto basketPostDto)
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
    private async Task<ResponseDto> UpdateCookieBasketItemAsync(BasketPutDto basketPutDto)
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

    private async Task<ShoppingSession> GetOrCreateUserSessionAsync(string userId) //OK
    {
        var userSession = await _shoppingSessionService.GetShoppingSessionByUserIdAsync(userId);

        return userSession ?? new ShoppingSession { UserId = userId };
    }

    #endregion

    #region Common Methods
    private async Task<string> GetUserIdAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _userService.GetUserByIdAsync(userId);
        return userId;
    } //OK

    private async Task IncludeBookToBasketItemAsync(List<BasketItem> cookieBasketItems) //--
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
        ShoppingSession? userSession = await _shoppingSessionService.GetShoppingSessionByUserIdAsync(userId) ?? new ShoppingSession();

        return userSession;
    } //OK

    #endregion
}
