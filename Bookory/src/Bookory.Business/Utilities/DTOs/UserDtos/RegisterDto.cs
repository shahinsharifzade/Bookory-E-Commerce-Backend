namespace Bookory.Business.Utilities.DTOs.UserDtos;

public record RegisterDto(string UserName, string FullName, string Email, string Password, string PasswordConfirm);
