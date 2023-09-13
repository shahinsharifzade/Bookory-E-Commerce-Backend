using Bookory.Core.Models.Identity;

namespace Bookory.Business.Utilities.DTOs.UserDtos;

public record UserAllDetailsGetResponseDto(AppUser User , string Role);