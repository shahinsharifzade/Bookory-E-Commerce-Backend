using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Core.Models.Identity;

namespace Bookory.Business.Services.Interfaces;

public interface IUserService
{
    Task<List<UserGetResponseDto>> GetAllUsersAsync(string? search);
    Task<UserRoleGetResponseDto> GetUserByIdAsync(string id);
    Task<UserRoleGetResponseDto> GetActiveUser();
    Task<UserAllDetailsGetResponseDto> GetUserAllDetailsByIdAsync(string id);
    Task<UserAllDetailsGetResponseDto> GetUserByUsernameAsync(string username);
    Task<ResponseDto> CreateUserAsync(RegisterDto userPostDto);
    Task<ResponseDto> ChangeUserRoleAsync(Guid userId , Guid roleId);
    Task<ResponseDto> UpdateUserAsync(AppUser user);
    Task<ResponseDto> ChangeUserActiveStatusAsync(string userId);
}
