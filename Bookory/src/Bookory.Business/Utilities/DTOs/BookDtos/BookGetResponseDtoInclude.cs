using Bookory.Business.Utilities.DTOs.BookImageDtos;
using Bookory.Business.Utilities.DTOs.GenreDtos;

namespace Bookory.Business.Utilities.DTOs.BookDtos;

public record BookGetResponseDtoInclude(Guid Id, string Title, string Description, string MainImage, decimal Price, decimal DiscountPrice, int Rating, ICollection<BookImageGetResponseDtoInclude> Images );