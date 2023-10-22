using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.ContactDtos;
using Bookory.Business.Utilities.DTOs.GenreDtos;
using Bookory.Business.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _contactService.GetAllContactMessagesAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await _contactService.GetContactMessageByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(ContactPostDto contactPostDto)
    {
        var response = await _contactService.CreateContactMessageAsync(contactPostDto);

        return StatusCode(response.StatusCode, response.Message);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _contactService.DeleteContactMessagesAsync(id);

        return StatusCode(response.StatusCode, response.Message);
    }
}
