using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.WishlistDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IWishlistService
{
    Task<WishlistGetResponseDto> GetAllWishlistAsync();
    Task<ResponseDto> AddItemToWishlist(Guid id);
    Task<ResponseDto> RemoveWishlistItem(Guid id);
    Task<ResponseDto> TransferCookieWishlistToDatabaseAsync(string userId);
}
