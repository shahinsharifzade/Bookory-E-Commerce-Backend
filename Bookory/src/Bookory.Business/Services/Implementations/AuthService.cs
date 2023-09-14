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
    private readonly IWishlistService _wishlistService;
    private readonly IBasketService _basketService;
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
        if (user is null) throw new LoginFailedException("User with the provided username does not exist.");

        if (!user.EmailConfirmed) throw new ConfirmEmailException("Login failed because the email associated with this account has not been confirmed");
        if (!user.IsActive) throw new BlockedUserException("Login failed because this account has been blocked");

        bool isSuccess = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isSuccess) throw new LoginFailedException("Invalid username or password. Please check your credentials.");

        await _basketService.TransferCookieBasketToDatabaseAsync(user.Id);
        await _wishlistService.TransferCookieWishlistToDatabaseAsync(user.Id);

        IList<Claim> claims = await _userManager.GetClaimsAsync(user);
        TokenResponseDto tokenResponseDto = _tokenHelper.CreateToken(claims.ToList());

        return tokenResponseDto;
    }

    public async Task<ResponseDto> ConfirmEmailAsync(string token, string email)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            throw new NullOrWhitespaceArgumentException("Email or token cannot be null or whitespace.");

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new UserNotFoundException($"User not found with email: {email}");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded) throw new UserUpdateFailedException($"Failed to confirm email: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        user.IsActive = true;

        return new ResponseDto((int)HttpStatusCode.OK, "Email has been successfully confirmed");
    }

    public async Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user is null) throw new UserNotFoundException($"User not found with email: {forgotPasswordDto.Email}");

        var httpContext = _httpContextAccessor.HttpContext;
        var request = httpContext.Request;
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string url = _linkGenerator.GetUriByAction(httpContext, "ResetPassword", "Auth", new { token = token, email = forgotPasswordDto.Email }, scheme: request.Scheme, host: request.Host)!;

        string emailSubject = "Password Reset Request";
        string emailBody = $@"
        <html>
        <body>
            <p>Hello,</p>
            <p>You have requested to reset your password for your account.</p>
            <p>To reset your password, click on the following link:</p>
            <p><a href='{HtmlEncoder.Default.Encode(url)}'>Reset Your Password</a></p>
            <p>If you did not request this password reset, you can safely ignore this email.</p>
            <p>Thank you!</p>
        </body>
        </html>";

        MailRequestDto mailRequestDto = new MailRequestDto(
            forgotPasswordDto.Email,
            emailSubject,
            emailBody,
            null
        );
        await _mailService.SendEmailAsync(mailRequestDto);
        return new ResponseDto((int)HttpStatusCode.OK, "A reset password link has been sent to your email");
    }

    public async Task<ResponseDto> ResetPasswordAsync(ChangePasswordDto resetPasswordDto, string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new UserNotFoundException($"User not found with the email: {email}");

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            throw new NullOrWhitespaceArgumentException("Email or token cannot be null or empty.");

        if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            throw new UserCreateFailedException("Passwords do not match.");

        var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.Password);
        if (!result.Succeeded)
            throw new UserCreateFailedException("Failed to reset password. Please check the following errors: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        return new ResponseDto((int)HttpStatusCode.OK, "Password updated successfully.");
    }
}
