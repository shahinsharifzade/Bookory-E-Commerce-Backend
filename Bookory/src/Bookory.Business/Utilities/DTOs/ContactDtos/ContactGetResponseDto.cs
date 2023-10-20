namespace Bookory.Business.Utilities.DTOs.ContactDtos;

public record ContactGetResponseDto(Guid Id, string Name, string Email, string Message, DateTime CreatedAt);