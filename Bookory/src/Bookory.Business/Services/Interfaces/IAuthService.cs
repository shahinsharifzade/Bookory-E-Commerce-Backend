using Bookory.Business.Utilities.DTOs.AuthDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
    Task<ResponseDto> ConfirmEmailAsync(string email, string token);
    Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<ResponseDto> ResetPasswordAsync(string token, string email);
    Task<ResponseDto> ResetPasswordAsync(ChangePasswordDto resetPasswordDto, string token, string email);
}
