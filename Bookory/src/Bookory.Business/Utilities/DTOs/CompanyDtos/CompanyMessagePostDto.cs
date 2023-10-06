namespace Bookory.Business.Utilities.DTOs.CompanyDtos;

public record CompanyMessagePostDto(string Name, string Email, string Message, Guid CompanyId);