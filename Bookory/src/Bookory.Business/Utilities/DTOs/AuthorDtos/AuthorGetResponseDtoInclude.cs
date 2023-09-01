using Bookory.Business.Utilities.DTOs.AuthorImageDtos;

namespace Bookory.Business.Utilities.DTOs.AuthorDtos;

public record AuthorGetResponseDtoInclude(Guid Id, string Name, string MainImage, string Biography, ICollection<AuthorImageGetResponseDtoInclude> Images);
