using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.UserAddressDtos;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class UserAddressController : ControllerBase
{
    private readonly IUserAddressService _userAddressService;

    public UserAddressController(IUserAddressService userAddressService)
    {
        _userAddressService = userAddressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response =await _userAddressService.GetAllAddressAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _userAddressService.GetAddressByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserAddressPostDto userAddressPostDto)
    {
        var response =await _userAddressService.AddAddressAsync(userAddressPostDto);
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Update(UserAddressPutDto userAddressPutDto)
    {
        var response = await _userAddressService.UpdateAddressAsync(userAddressPutDto);
        return Ok(response);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Update(Guid id)
    {
        var response = await _userAddressService.DeleteAddressAsync(id);
        return Ok(response);
    }
}
