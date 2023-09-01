using Bookory.Business.Utilities.DTOs.AuthorImageDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;

namespace Bookory.Business.Utilities.DTOs.AuthorDtos;

public record AuthorGetResponseDto(Guid Id, string Name,  string MainImage, string Biography, List<AuthorImageGetResponseDtoInclude> Images, List<BookGetResponseDto> Books);

