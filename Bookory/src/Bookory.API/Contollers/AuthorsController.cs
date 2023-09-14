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
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _authorService.GetAllAuthorsAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        return Ok(await _authorService.GetAuthorByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] AuthorPostDto authorPostDto)
    {
        var response = await _authorService.CreateAuthorAsync(authorPostDto);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromForm] AuthorPutDto authorPutDto)
    {
        var updatedAuthor = new AuthorPutDto(id, authorPutDto.Name, authorPutDto.Images, authorPutDto.MainImageIndex, authorPutDto.Biography);
        var response = await _authorService.UpdateAuthorAsync(updatedAuthor);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _authorService.DeleteAuthorAsync(id);

        return StatusCode(response.StatusCode, response.Message);
    }

}
