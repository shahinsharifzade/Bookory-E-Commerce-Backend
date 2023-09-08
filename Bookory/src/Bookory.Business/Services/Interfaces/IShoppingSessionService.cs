using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Core.Models;

namespace Bookory.Business.Services.Interfaces;

public interface IShoppingSessionService
{
    Task<ShoppingSession> GetShoppingSessionByUserIdAsync(string id);
    Task<bool> ShoppingSessionIsExistAsync(Guid id);
    //Task<ResponseDto> CreateShoppingSessionAsync(ShoppingSession shoppingSession);
    Task<ResponseDto> UpdateShoppingSessionAsync(ShoppingSession shoppingSession);

}
