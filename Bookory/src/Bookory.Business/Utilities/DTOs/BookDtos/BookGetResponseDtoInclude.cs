using Bookory.Business.Utilities.DTOs.BookImageDtos;

namespace Bookory.Business.Utilities.DTOs.BookDtos;

public record BookGetResponseDtoInclude(Guid Id, string Title, string Description, string MainImage, decimal Price, decimal DiscountPercentage, decimal Rating, ICollection<BookImageGetResponseDtoInclude> Images ); 