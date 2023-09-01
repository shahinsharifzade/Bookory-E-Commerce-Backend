namespace Bookory.Business.Utilities.DTOs.AuthorImageDtos;

public record AuthorImageGetResponseDtoInclude(string Image, bool IsMain, Guid AuthorId);
