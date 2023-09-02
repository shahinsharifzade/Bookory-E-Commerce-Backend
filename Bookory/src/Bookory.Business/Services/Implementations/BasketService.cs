using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class BasketService : IBasketService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IShoppingSessionRepository _shoppingSessionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly bool _isAuthenticated;


    public BasketService(UserManager<AppUser> userManager, IBasketItemRepository basketItemRepository, IShoppingSessionRepository shoppingSessionRepository, IHttpContextAccessor httpContextAccessor, IBookRepository bookRepository, IMapper mapper)
    {
        _userManager = userManager;
        _basketItemRepository = basketItemRepository;
        _shoppingSessionRepository = shoppingSessionRepository;
        _httpContextAccessor = httpContextAccessor;
        _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<ResponseDto> AddItemToBasketAsync(BasketPostDto basketPostDto)
    {
        EnsureAuthenticated();
        ValidateInput(basketPostDto);

        var book = await GetBookByIdAsync(basketPostDto);
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
                SessionId = userSession.Id,
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

    private async Task<string> GetUserIdAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new Exception($"User can not find by");
        return userId;
    }

    private async Task<Book> GetBookByIdAsync(BasketPostDto basketPostDto)
    {
        var book = await _bookRepository.GetByIdAsync(basketPostDto.Id);
        if (book is null)
            throw new Exception("");
        return book;
    }

    private static void ValidateInput(BasketPostDto basketPostDto)
    {
        if (basketPostDto.Quantity <= 0)
            throw new Exception("Invalid Quantity");

        if (basketPostDto is null)
            throw new Exception("Invalid input data");
    }

    private void EnsureAuthenticated()
    {
        if (!_isAuthenticated)
            throw new Exception("Login !");
    }
    #endregion


    public async Task<List<BasketGetResponseDto>> GetActiveUserBasket()
    {
        if (!_isAuthenticated)
            throw new Exception("Login !");

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new Exception($"User can not find by");

        ShoppingSession? userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered,
        nameof(ShoppingSession.BasketItems), // Include BasketItems
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book), // Include Book
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Images), // Include Book Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Author) + "." + nameof(Author.Images), // Include Author Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.BookGenres) + "." + nameof(BookGenre.Genre)) ?? new ShoppingSession();

        var basketItems = _mapper.Map<List<BasketGetResponseDto>>(userSession.BasketItems);

        return basketItems;
    }

    public async Task<List<BasketGetResponseDto>> GetBasketItemAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new Exception($"User can not find by");

        ShoppingSession? userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == user.Id && !ss.IsOrdered,
        nameof(ShoppingSession.BasketItems), // Include BasketItems
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book), // Include Book
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Images), // Include Book Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Author) + "." + nameof(Author.Images), // Include Author Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.BookGenres) + "." + nameof(BookGenre.Genre)) ?? new ShoppingSession();

        var basketItems = _mapper.Map<List<BasketGetResponseDto>>(userSession.BasketItems);

        return basketItems;
    }

    public async Task<ResponseDto> RemoveBasketItemAsync(Guid id)
    {
        if (!_isAuthenticated)
            throw new Exception("Login !");

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new Exception($"User can not find by");

        ShoppingSession? userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered);
        //var isExist = await _basketItemRepository.IsExistAsync(bi => bi.BookId == id);
        var book = await _basketItemRepository.GetSingleAsync(bi => bi.BookId == id);
        if (book is null)
            throw new Exception($"Book does not exist in basket");

        _basketItemRepository.Delete(book);
        await _basketItemRepository.SaveAsync();
        return new ResponseDto((int)HttpStatusCode.OK, "Product is deleted");
    }

    public Task<ResponseDto> UpdateItemAsync(BasketPutDto basketPutDto)
    {
        throw new NotImplementedException();
    }
}
