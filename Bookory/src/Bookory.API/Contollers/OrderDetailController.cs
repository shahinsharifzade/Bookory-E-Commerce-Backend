using Bookory.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;

    public OrderDetailController(IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _orderDetailService.GetOrderDetailAsync(id);

        return Ok(response);    
    }

    [HttpGet("byUserId/{id}")]
    public async Task<IActionResult> GetByUserId(string id)
    {
        var response = await _orderDetailService.GetAllOrderDetailByUserIdAsync(id);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _orderDetailService.GetAllOrderDetailAsync();

        return Ok(response);
    }
}
