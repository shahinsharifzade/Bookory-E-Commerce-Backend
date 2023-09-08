using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Core.Models;

namespace Bookory.Business.Services.Interfaces;

public interface IBasketItemService
{
    Task<BasketItem> GetBasketItemByBookIdAsync(Guid id);
    Task<List<BasketItem>> GetBasketItemBySessionIdAsync(Guid id);
    Task<BasketItem> GetExistingBasketItemAsync(Guid userSessionId, Guid bookId);
    Task<ResponseDto> UpdateBasketItemAsync(BasketItem basketItem);
    Task<ResponseDto> DeleteBasketItemAsync(Guid id);
}
