using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.WishlistDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IWishlistService
{
    Task<WishlistGetResponseDto> GetAllWishlistAsync();
    Task<ResponseDto> AddItemToWishlist(WishlistPostDto wishlistPostDto);
    Task<ResponseDto> RemoveWishlistItem(Guid id);
    Task<ResponseDto> TransferCookieWishlistToDatabaseAsync(string userId);
    Task<ResponseDto> CheckItemExistsInWishlistAsync(Guid id);
}
