using Bookory.Business.Services.Interfaces;
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

    [HttpPost("{id}")]
    public async Task<IActionResult> Post(Guid id)
    {
        var response = await _wishlistService.AddItemToWishlist(id);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _wishlistService.RemoveWishlistItem(id);
        return Ok(response);
    }
}
