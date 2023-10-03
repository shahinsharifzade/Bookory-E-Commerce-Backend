using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.DTOs.BookImageDtos;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.DTOs.GenreDtos;

namespace Bookory.Business.Utilities.DTOs.BookDtos;

public record BookGetResponseDto(Guid Id, string Title, int StockQuantity, int SoldQuantity, string Description, decimal Price, decimal DiscountPrice, decimal Rating, int NumberOfRatings, string MainImage, CompanyGetResponseDtoInclude Company, ICollection<BookImageGetResponseDtoInclude> Images, AuthorGetResponseDtoInclude Author, ICollection<GenreGetResponseDtoInclude> Genres);

