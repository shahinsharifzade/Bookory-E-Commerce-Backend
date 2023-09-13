namespace Bookory.Business.Utilities.DTOs.UserDtos;

public record UserRoleGetResponseDto(UserGetResponseDto User, string Role, bool? IsVendorRegistrationComplete);
