using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.MailDtos;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Business.Utilities.Email;
using Bookory.Business.Utilities.Enums;
using Bookory.Business.Utilities.Exceptions.AuthException;
using Bookory.Business.Utilities.Exceptions.RoleException;
using Bookory.Business.Utilities.Exceptions.UserException;
using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Bookory.Business.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _usermanager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    public UserService(UserManager<AppUser> usermanager, IMapper mapper, RoleManager<IdentityRole> roleManager, IMailService mailService, LinkGenerator linkGenerator, IHttpContextAccessor httpContext)
    {
        _usermanager = usermanager;
        _mapper = mapper;
        _roleManager = roleManager;
        _mailService = mailService;
        _linkGenerator = linkGenerator;
        _httpContext = httpContext;
    }

    public async Task<ResponseDto> CreateUserAsync(RegisterDto userPostDto)
    {
        if (userPostDto.Password != userPostDto.PasswordConfirm)
            throw new UserCreateFailedException("The provided password and password confirmation do not match.");

        AppUser appUser = await _usermanager.FindByNameAsync(userPostDto.UserName);

        if (appUser != null)
            throw new UserAlreadyExistException("A user with the provided username already exists.");

        var newUser = _mapper.Map<AppUser>(userPostDto);
        var result = await _usermanager.CreateAsync(newUser, userPostDto.Password);
        if (!result.Succeeded)
            throw new UserCreateFailedException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        //await CreateRolesAsync();

        if (userPostDto.RegisterAsVendor)
        {
            await _usermanager.AddToRoleAsync(newUser, Roles.Vendor.ToString());
            newUser.IsVendorRegistrationComplete = false;
            await _usermanager.UpdateAsync(newUser);
        }
        else
            await _usermanager.AddToRoleAsync(newUser, Roles.Member.ToString());


        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.NameIdentifier , newUser.Id),
            new Claim(ClaimTypes.Name, newUser.UserName),
            new Claim(ClaimTypes.Email , newUser.Email),
            new Claim("FullName" , newUser.FullName!)
        };

        // Add Role Claim to Claims
        var userRoles = await _usermanager.GetRolesAsync(newUser);
        foreach (var userRole in userRoles)
        {
            userClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        await _usermanager.AddClaimsAsync(newUser, userClaims);

        string token = await _usermanager.GenerateEmailConfirmationTokenAsync(newUser);
        var httpContext = _httpContext.HttpContext;
        var request = httpContext.Request;
        string url = _linkGenerator.GetUriByAction(httpContext, "Verify", "Auth", new { token = token, email = userPostDto.Email }, scheme: request.Scheme, host: new HostString("localhost", 3000))!;

        MailRequestDto mailRequestDto = new(
            newUser.Email,
            "Confirm Your Email",
            $"<a href='{HtmlEncoder.Default.Encode(url)}'>Click here to confirm your email address</a>",
            null
            );

        await _mailService.SendEmailAsync(mailRequestDto);

        return new ResponseDto((int)HttpStatusCode.Created, "Registration completed successfully.");
    }

    public async Task<List<UserGetResponseDto>> GetAllUsersAsync(string? search)
    {
        var users = await _usermanager.Users.Where(u => search == null || u.UserName.ToLower().Contains(search.Trim().ToLower())).ToListAsync();
        var userDtos = _mapper.Map<List<UserGetResponseDto>>(users);
        return userDtos;
    }

    public async Task<UserRoleGetResponseDto> GetUserByIdAsync(string id)
    {
        var user = await _usermanager.FindByIdAsync(id);

        if (user is null)
            throw new UserNotFoundException($"No user was found with the specified ID: {id}");

        var userDto = _mapper.Map<UserGetResponseDto>(user);
        var userRoles = await _usermanager.GetRolesAsync(user);

        UserRoleGetResponseDto userRoleDto = new(userDto, userRoles.FirstOrDefault()!, user.IsVendorRegistrationComplete);
        return userRoleDto;
    }

    public async Task<UserAllDetailsGetResponseDto> GetUserByUsernameAsync(string username)
    {
        var user = await _usermanager.FindByNameAsync(username);

        if (user is null)
            throw new UserNotFoundException($"No user was found with the specified username: {username}");


        var userRoles = await _usermanager.GetRolesAsync(user);

        UserAllDetailsGetResponseDto userRoleDto = new(user, userRoles.FirstOrDefault()!);
        return userRoleDto;
    }

    public async Task<ResponseDto> ChangeUserRoleAsync(Guid userId, Guid roleId)
    {
        var user = await _usermanager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new UserNotFoundException($"User with ID {userId} not found.");

        var userRoles = await _usermanager.GetRolesAsync(user);
        if (userRoles.FirstOrDefault() == "Admin")
            throw new RoleChangeNotAllowedException("Changing roles for administrators is not permitted.");

        await _usermanager.RemoveFromRolesAsync(user, userRoles);

        var newRole = await _roleManager.FindByIdAsync(roleId.ToString());
        if (newRole is null) throw new RoleNotFoundException($"Role with ID {roleId} not found.");

        await _usermanager.AddToRoleAsync(user, newRole.ToString());

        // Update claims with the new role
        var userClaims = await _usermanager.GetClaimsAsync(user);
        var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (roleClaim != null)
        {
            await _usermanager.RemoveClaimAsync(user, roleClaim);
        }

        // Add the new role as a claim
        await _usermanager.AddClaimAsync(user, new Claim(ClaimTypes.Role, newRole.Name));

        return new((int)HttpStatusCode.OK, "User role changed successfully.");
    }

    private async Task CreateRolesAsync()
    {
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
                await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        }
    }

    public async Task<ResponseDto> UpdateUserAsync(AppUser user)
    {
        await _usermanager.UpdateAsync(user);
        return new ResponseDto((int)HttpStatusCode.Created, "User update successfully.");
    }

    public async Task<UserAllDetailsGetResponseDto> GetUserAllDetailsByIdAsync(string id)
    {
        var user = await _usermanager.FindByIdAsync(id);

        if (user is null)
            throw new UserNotFoundException($"No user was found with the specified Id: {id}");

        var userRoles = await _usermanager.GetRolesAsync(user);

        UserAllDetailsGetResponseDto userRoleDto = new(user, userRoles.FirstOrDefault()!);
        return userRoleDto;
    }

    public async Task<UserRoleGetResponseDto> GetActiveUser()
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _usermanager.FindByIdAsync(userId);


        var userDto = _mapper.Map<UserGetResponseDto>(user);

        var userRoles = await _usermanager.GetRolesAsync(user);

        UserRoleGetResponseDto userRoleDto = new(userDto, userRoles.FirstOrDefault()!, user.IsVendorRegistrationComplete);
        return userRoleDto;
    }


    public async Task<ResponseDto> ChangeUserActiveStatusAsync(string userId)
    {
        var user = await _usermanager.FindByIdAsync(userId);

        if (user == null)
            throw new UserNotFoundException($"No user was found with the specified ID: {userId}");

        user.IsActive = !user.IsActive;

        var result = await _usermanager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new UserUpdateFailedException("Failed to update user's active status.");

        return new ResponseDto((int)HttpStatusCode.OK, "User active status updated successfully.");
    }



}
