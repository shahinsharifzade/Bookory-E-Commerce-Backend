using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.AuthDtos;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var response = await _authService.LoginAsync(loginDto);
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Verify(string token, string email)
    {  
        var response = await _authService.ConfirmEmailAsync(token, email);
        return Ok(response);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var response = await _authService.ForgotPasswordAsync(forgotPasswordDto);
        return Ok(response);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto resetPasswordDto, [FromQuery] string token, [FromQuery] string email)
    {
        var response = await _authService.ResetPasswordAsync(resetPasswordDto, token, email);
        return Ok(response);
    }

}
