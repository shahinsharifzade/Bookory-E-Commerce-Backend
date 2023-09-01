using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookory.Business.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleService(IMapper mapper, RoleManager<IdentityRole> roleManager)
    {
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<List<RoleGetResponseDto>> GetAllRolesAsync(string? search)
    {
        var roles = await _roleManager.Roles.Where(r => search != null ? r.Name.ToLower().Contains(search.Trim().ToLower()) : true).ToListAsync();
        var rolesDto = _mapper.Map<List<RoleGetResponseDto>>(roles);

        return rolesDto;
    }
}
