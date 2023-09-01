namespace Bookory.Business.Utilities.DTOs.BookImageDtos;

public record BookImageGetResponseDtoInclude(string Image, bool IsMain, Guid BookId);
