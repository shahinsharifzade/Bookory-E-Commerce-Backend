using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.DTOs.BookImageDtos;
using Bookory.Business.Utilities.DTOs.GenreDtos;

namespace Bookory.Business.Utilities.DTOs.BookDtos;

public record BookGetResponseDto(Guid Id, string Title, int StockQuantity, string Description, decimal Price, decimal DiscountPrice, int Rating, string MainImage,
    ICollection<BookImageGetResponseDtoInclude> Images, AuthorGetResponseDtoInclude Author, ICollection<GenreGetResponseDtoInclude> Genres);

