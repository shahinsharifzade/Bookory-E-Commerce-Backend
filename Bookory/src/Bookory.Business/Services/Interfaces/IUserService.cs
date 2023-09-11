using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.UserDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IUserService
{
    Task<List<UserGetResponseDto>> GetAllUsersAsync(string? search);
    Task<UserRoleGetResponseDto> GetUserByIdAsync(string id);
    Task<ResponseDto> CreateUserAsync(RegisterDto userPostDto);
    Task<ResponseDto> ChangeUserRoleAsync(Guid userId , Guid roleId);
}
