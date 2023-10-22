using Bookory.Business.Utilities.DTOs.BookImageDtos;
using Bookory.Business.Utilities.Enums;

namespace Bookory.Business.Utilities.DTOs.BookDtos;

public record BookGetResponseDtoInclude(Guid Id, string Title, string Description, string MainImage, decimal Price, decimal DiscountPercentage, decimal Rating, BookStatus Status, ICollection<BookImageGetResponseDtoInclude> Images ); 