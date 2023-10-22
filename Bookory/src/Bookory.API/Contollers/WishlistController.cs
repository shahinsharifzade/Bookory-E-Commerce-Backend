using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.WishlistDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _wishlistService.GetAllWishlistAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromQuery] WishlistPostDto wishlistPostDto)
    {
        var response = await _wishlistService.AddItemToWishlist(wishlistPostDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _wishlistService.RemoveWishlistItem(id);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> IsExist (Guid id)
    {
        var response =await _wishlistService.CheckItemExistsInWishlistAsync(id);

        return Ok(response);
    }
}
