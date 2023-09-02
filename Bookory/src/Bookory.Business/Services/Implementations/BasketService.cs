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
        if (!_isAuthenticated)
            throw new Exception("Login !");

        if (basketPostDto.Quantity <= 0)
            throw new Exception("Invalid Quantity");

        if (basketPostDto is null)
            throw new Exception("Invalid input data");

        var book = await _bookRepository.GetByIdAsync(basketPostDto.Id);
        if (book is null)
            throw new Exception("");

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new Exception($"User can not find by");


        //ShoppingSession? userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered,
        //nameof(ShoppingSession.BasketItems), // Include BasketItems
        //nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book), // Include Book
        //nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Images), // Include Book Images
        //nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Author) + "." + nameof(Author.Images), // Include Author Images
        //nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.BookGenres) + "." + nameof(BookGenre.Genre)) ?? new ShoppingSession();

        ShoppingSession? userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered) ?? new ShoppingSession();

        BasketItem basketItem = new BasketItem
        {
            BookId = book.Id,
            Price = book.Price,
            Quantity = basketPostDto.Quantity,
            SessionId = userSession.Id,
        };

        userSession.BasketItems.Add(basketItem);
        userSession.UserId = userId;
        userSession.TotalPrice = userSession.BasketItems.Sum(p => p.Quantity * p.Price);

        await _shoppingSessionRepository.CreateAsync(userSession);
        await _shoppingSessionRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.Created, "Book successfully added");
    }

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
