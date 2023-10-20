using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.GenreDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _genreService.GetAllGenresAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById( [FromRoute] Guid id)
    {
        return Ok( await _genreService.GetGenreByIdAsync(id) );
    }

    [HttpPost]
    public async Task<IActionResult> Post(GenrePostDto genrePostDto)
    {
        var response = await _genreService.CreateGenreAsync(genrePostDto);

        return StatusCode( response.StatusCode , response.Message );
    }

    [HttpPut]
    public async Task<IActionResult> Put(GenrePutDto genrePutDto)
    {
        var response = await _genreService.UpdateGenreAsync(genrePutDto);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete( Guid id)
    {
        var response =await _genreService.DeleteGenreAsync(id);

        return StatusCode( response.StatusCode, response.Message );
    }

}
