using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.UserAddressDtos;
using Bookory.Business.Utilities.Exceptions.AuthException;
using Bookory.Business.Utilities.Exceptions.UserAddressException;
using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class UserAddressService : IUserAddressService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAdressRepository _userAdressRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserService _userService;
    private readonly bool _isAuthenticated;

    public UserAddressService(IMapper mapper, IUserAdressRepository userAdressRepository, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IUserService userService)
    {
        _mapper = mapper;
        _userAdressRepository = userAdressRepository;
        _httpContextAccessor = httpContextAccessor;
        _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        _userManager = userManager;
        _userService = userService;
    }

    public async Task<List<UserAddressGetReponseDto>> GetAllAddressAsync()
    {
        EnsureAuthenticated();
        string? userId = await GetUserIdAsync();

        var address = await _userAdressRepository.GetFiltered(ua => ua.UserId == userId, nameof(UserAddress.User)).ToListAsync();
        var addressDto = _mapper.Map<List<UserAddressGetReponseDto>>(address);

        return addressDto;
    }

    public async Task<UserAddressGetReponseDto> GetAddressByIdAsync(Guid id)
    {
        EnsureAuthenticated();

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            throw new UserNotFoundException("User not found");

        var address = await _userAdressRepository.GetSingleAsync(ua => ua.Id == id, nameof(UserAddress.User));
        if(address is null)
            throw new UserAddressNotFoundException("User Address not found");

        var addressDto = _mapper.Map<UserAddressGetReponseDto>(address);

        return addressDto;
    }

    private async Task<UserAddress> GetUserAddressAsync(UserAddressPutDto userAddressPutDto)
    {
        var address = await _userAdressRepository.GetSingleAsync(ua => ua.Id == userAddressPutDto.Id);
        if (address is null)
            throw new UserAddressNotFoundException("User Address not found");
        return address;
    }

    public async Task<ResponseDto> AddAddressAsync(UserAddressPostDto userAddressPostDto)
    {
        EnsureAuthenticated();
        var userId = await GetUserIdAsync();

        var newAddressDto = _mapper.Map<UserAddress>(userAddressPostDto);
        newAddressDto.UserId = userId;

        await _userAdressRepository.CreateAsync(newAddressDto);
        await _userAdressRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.Created, "User Address successfully created");
    }

    public async Task<ResponseDto> UpdateAddressAsync(UserAddressPutDto userAddressPutDto)
    {
        EnsureAuthenticated();
        var userId = await GetUserIdAsync();
        UserAddress? address = await GetUserAddressAsync(userAddressPutDto);

        var updatedAddress = _mapper.Map(userAddressPutDto, address);
        updatedAddress.UserId = userId;
        _userAdressRepository.Update(updatedAddress);
        await _userAdressRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "User Address successfully update");
    }

    public async Task<ResponseDto> DeleteAddressAsync(Guid id)
    {
        EnsureAuthenticated();

        var address = await _userAdressRepository.GetSingleAsync(ua => ua.Id == id);
        if (address is null)
            throw new UserAddressNotFoundException("User Address not found");

        _userAdressRepository.SoftDelete(address);
        await _userAdressRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "User Address successfully deleted");
    }
    
    private void EnsureAuthenticated()
    {
        if (!_isAuthenticated)
            throw new AuthenticationFailedException("Please Login");

    }
    private async Task<string> GetUserIdAsync()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetUserByIdAsync(userId);

        if (user is null)
            throw new UserNotFoundException("User not found");

        return userId;
    }
}
