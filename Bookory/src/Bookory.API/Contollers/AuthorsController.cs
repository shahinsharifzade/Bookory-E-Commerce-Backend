using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _authorService.GetAllAuthorsAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        return Ok(await _authorService.GetAuthorByIdAsync(id));
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return Ok(await _authorService.GetPageOfAuthorsAsync(pageNumber, pageSize));
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Post([FromForm] AuthorPostDto authorPostDto)
    {
        var response = await _authorService.CreateAuthorAsync(authorPostDto);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpPut]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> Put([FromForm] AuthorPutDto authorPutDto)
    {
        var response = await _authorService.UpdateAuthorAsync(authorPutDto);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _authorService.DeleteAuthorAsync(id);

        return StatusCode(response.StatusCode, response.Message);
    }
}
