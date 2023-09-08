using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.Stripe;
using Bookory.Core.Models.Stripe;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public StripeController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    //[HttpPost("customer")]
    //public async Task<ActionResult<CustomerResource>> CreateCustomer([FromBody] CustomerResourcePostDto resource,
    //    CancellationToken cancellationToken)
    //{
    //    var response = await _stripeService.CreateCustomer(resource, cancellationToken);
    //    return Ok(response);
    //}

    //[HttpPost("charge")]
    //public async Task<ActionResult<ChargeResource>> CreateCharge([FromBody] ChargeResourcePostDto resource, CancellationToken cancellationToken)
    //{
    //    var response = await _stripeService.CreateCharge(resource, cancellationToken);
    //    return Ok(response);
    //}
}
