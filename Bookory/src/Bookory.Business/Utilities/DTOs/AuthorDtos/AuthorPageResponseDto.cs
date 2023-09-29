namespace Bookory.Business.Utilities.DTOs.AuthorDtos;

public record AuthorPageResponseDto(ICollection<AuthorGetResponseDto> Authors, decimal TotalCount);