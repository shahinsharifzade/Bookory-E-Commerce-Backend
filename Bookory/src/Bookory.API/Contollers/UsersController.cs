using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Business.Utilities.Exceptions.AuthException;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> GetById(string id)
    {
        return Ok(await _userService.GetUserByIdAsync(id));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveUser()
    {
        return Ok(await _userService.GetActiveUser());
    }

    [HttpPut("changerole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole([FromQuery] Guid userId,[FromQuery] Guid roleId)
    {
        var response = await _userService.ChangeUserRoleAsync(userId, roleId);
        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpPut("changeActiveStatus/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeActiveStatus(string id)
    {
        var response = await _userService.ChangeUserActiveStatusAsync(id);
        return StatusCode(response.StatusCode, response.Message);
    }

}
