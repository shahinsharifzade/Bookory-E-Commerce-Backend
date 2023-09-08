using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.ShoppingSessionDtos;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class ShoppingSessionService : IShoppingSessionService
{
    private readonly IMapper _mapper;
    private readonly IShoppingSessionRepository _shoppingSessionRepository;
    public ShoppingSessionService(IMapper mapper, IShoppingSessionRepository shoppingSessionRepository)
    {
        _mapper = mapper;
        _shoppingSessionRepository = shoppingSessionRepository;
    }

    public async Task<ShoppingSession> GetShoppingSessionByUserIdAsync(string id)
    {
        var shoppingSession = await _shoppingSessionRepository.GetSingleAsync(ss => ss.UserId == id && !ss.IsOrdered,
        nameof(ShoppingSession.BasketItems), // Include BasketItems
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book), // Include Book
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Images), // Include Book Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.Author) + "." + nameof(Author.Images), // Include Author Images
        nameof(ShoppingSession.BasketItems) + "." + nameof(BasketItem.Book) + "." + nameof(Book.BookGenres) + "." + nameof(BookGenre.Genre));

        return shoppingSession;
    }

    public Task<bool> ShoppingSessionIsExistAsync(Guid id)
    {
        var isExist = _shoppingSessionRepository.IsExistAsync(ss => ss.Id == id);   
        return isExist;
    }

    public async Task<ResponseDto> UpdateShoppingSessionAsync(ShoppingSession shoppingSession)
    {
        _shoppingSessionRepository.Update(shoppingSession);
        await _shoppingSessionRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "Shopping Session Updated");
    }
}
