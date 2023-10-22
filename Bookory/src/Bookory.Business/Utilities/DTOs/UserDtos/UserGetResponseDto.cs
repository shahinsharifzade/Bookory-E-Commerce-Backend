using Bookory.Core.Models;

namespace Bookory.Business.Utilities.DTOs.UserDtos;

public record UserGetResponseDto(string Id, string FullName, string Email, string UserName, bool IsActive);
