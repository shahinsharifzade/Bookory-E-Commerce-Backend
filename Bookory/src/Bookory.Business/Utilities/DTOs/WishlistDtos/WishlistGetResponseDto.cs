using Bookory.Business.Utilities.DTOs.BookDtos;
namespace Bookory.Business.Utilities.DTOs.WishlistDtos;

public record WishlistGetResponseDto(Guid? Id = null, string? UserId = null, List<BookGetResponseDto>? Books = null);
