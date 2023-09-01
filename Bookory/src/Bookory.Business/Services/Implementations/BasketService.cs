using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class BasketService : IBasketService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IShoppingSessionRepository _shoppingSessionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBookRepository _bookRepository;
    private readonly bool _isAuthenticated;


    public BasketService(UserManager<AppUser> userManager, IBasketItemRepository basketItemRepository, IShoppingSessionRepository shoppingSessionRepository, IHttpContextAccessor httpContextAccessor, IBookRepository bookRepository)
    {
        _userManager = userManager;
        _basketItemRepository = basketItemRepository;
        _shoppingSessionRepository = shoppingSessionRepository;
        _httpContextAccessor = httpContextAccessor;
        _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        _bookRepository = bookRepository;
    }

    public async Task<ResponseDto> AddItemToBasketAsync(BasketPostDto basketPostDto)
    {
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

        var userSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == userId && !ss.IsOrdered, nameof(AppUser));

        if (userSession == null)
        {
            ShoppingSession newShoppingSession = new ShoppingSession();
            newShoppingSession.UserId = userId;
            newShoppingSession.TotalPrice = 0;
            await _shoppingSessionRepository.CreateAsync(newShoppingSession);
            await _shoppingSessionRepository.SaveAsync();
        }

        BasketItem basketItem = new BasketItem { 
            BookId = book.Id,
            Quantity = basketPostDto.Quantity,
            Price = book.Price
        };


        //else
        //{
        //    shoppingSessionId = userSession.FirstOrDefault(c => c.);
        //}
        throw new NotImplementedException();


    }

    public ShoppingSession GetActiveUserBasket()
    {
        throw new NotImplementedException();
    }

    public Task<List<BasketGetResponseDto>> GetBasketItemAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> RemoveBasketItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UpdateItemAsync(BasketPutDto basketPutDto)
    {
        throw new NotImplementedException();
    }
}
