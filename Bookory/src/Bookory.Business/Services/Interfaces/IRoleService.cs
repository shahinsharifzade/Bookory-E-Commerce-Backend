using Bookory.Business.Utilities.DTOs.UserDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IRoleService
{
    Task<List<RoleGetResponseDto>> GetAllRolesAsync(string? search);

}
