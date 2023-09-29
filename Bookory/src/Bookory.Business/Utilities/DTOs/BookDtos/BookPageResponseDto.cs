namespace Bookory.Business.Utilities.DTOs.BookDtos;

public record BookPageResponseDto(ICollection<BookGetResponseDto> Books , decimal TotalCount);