using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.AuthDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.MailDtos;
using Bookory.Business.Utilities.Email;
using Bookory.Business.Utilities.Exceptions.AuthException;
using Bookory.Business.Utilities.Exceptions.Common;
using Bookory.Business.Utilities.Exceptions.LoginException;
using Bookory.Business.Utilities.Exceptions.UserException;
using Bookory.Business.Utilities.Security.JWT.Interface;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Bookory.Business.Services.Implementations;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBasketService _basketService;
    private readonly IWishlistService _wishlistService;
    private readonly ITokenHelper _tokenHelper;
    private readonly IMailService _mailService;

    public AuthService(UserManager<AppUser> userManager, ITokenHelper tokenHelper, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator, IMailService mailService, IBasketService basketService, IWishlistService wishlistService)
    {
        _userManager = userManager;
        _tokenHelper = tokenHelper;
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
        _mailService = mailService;
        _basketService = basketService;
        _wishlistService = wishlistService;
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
        {
        AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user is null) throw new LoginFailedException("Invalid username or password");

        bool isSuccess = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isSuccess) throw new LoginFailedException("Invalid username or password");

        await _basketService.TransferCookieBasketToDatabaseAsync(user.Id);
        await _wishlistService.TransferCookieWishlistToDatabaseAsync(user.Id);

        IList<Claim> claims = await _userManager.GetClaimsAsync(user);
        TokenResponseDto tokenResponseDto = _tokenHelper.CreateToken(claims.ToList());

        return tokenResponseDto;
    }

    public async Task<ResponseDto> ConfirmEmailAsync(string token, string email)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            throw new NullOrWhitespaceArgumentException("email or token null");

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new UserNotFoundException($"User not found by email: {email}");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded) throw new UserUpdateFailedException(string.Join(", ", result.Errors.Select(e => e.Description)));
        user.IsActive = true;

        return new ResponseDto((int)HttpStatusCode.OK, "Email is confirmed");
    }

    public async Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user is null) throw new UserNotFoundException($"User not found by email: {forgotPasswordDto.Email}");

        var httpContext = _httpContextAccessor.HttpContext;
        var request = httpContext.Request;
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string url = _linkGenerator.GetUriByAction(httpContext, "ResetPassword", "Auth", new { token = token, email = forgotPasswordDto.Email }, scheme: request.Scheme, host: request.Host)!;

        MailRequestDto mailRequesDto = new(
        forgotPasswordDto.Email,
        "Reset Your Password",
        $"<a href='{HtmlEncoder.Default.Encode(url)}'>Reset Your Password</a>",
        null
        );

        await _mailService.SendEmailAsync(mailRequesDto);
        return new ResponseDto((int)HttpStatusCode.OK, "Reset Password Link is ready");
    }

    public async Task<ResponseDto> ResetPasswordAsync(string token, string email)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(email))
            throw new NullOrWhitespaceArgumentException("email or token null");

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) throw new UserNotFoundException($"User not found by email: {email}");

        return new ResponseDto((int)HttpStatusCode.OK, $"Everything is okay. token: {token} ");
    }

    public async Task<ResponseDto> ResetPasswordAsync(ChangePasswordDto resetPasswordDto, string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            if (user is null) throw new UserNotFoundException($"User not found by email: {email}");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            throw new NullOrWhitespaceArgumentException("email or token null");

        if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            throw new UserCreateFailedException("Password and PasswordConfirm do not match");

        var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.Password);
        if (!result.Succeeded) 
            throw new UserCreateFailedException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return new ResponseDto((int)HttpStatusCode.OK, "Password Updated Succeffuly");
    }
}
