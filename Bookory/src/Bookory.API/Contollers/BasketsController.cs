using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketsController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var basket = await _basketService.GetActiveUserBasket();
        return Ok(basket);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromQuery] BasketPostDto basketPostDto)
    {
        var respons =await _basketService.AddItemToBasketAsync(basketPostDto);
        return Ok(respons);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var respons = await _basketService.RemoveBasketItemAsync(id);
        return Ok(respons);
    }
}
