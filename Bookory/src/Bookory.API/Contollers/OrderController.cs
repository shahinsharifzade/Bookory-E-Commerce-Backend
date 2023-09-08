using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.OrderDtos;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(OrderPostDto orderPostDto)
    {
        var response =await _orderService.PurchaseBooks(orderPostDto);

        return Ok(response);
    }
}
