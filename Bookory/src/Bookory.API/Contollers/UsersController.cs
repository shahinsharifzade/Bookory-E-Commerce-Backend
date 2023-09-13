using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Business.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var response = await _userService.CreateUserAsync(registerDto);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _userService.GetAllUsersAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById( string id)
    {
        return Ok(await _userService.GetUserByIdAsync(id));
    }

    [HttpPut("changerole")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole(Guid userId, Guid roleId)
    {
        var response = await _userService.ChangeUserRoleAsync(userId, roleId);
        return StatusCode(response.StatusCode, response.Message);
    }


}
