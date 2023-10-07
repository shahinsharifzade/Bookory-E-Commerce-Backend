using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search)
    {
        return Ok(await _blogService.GetAllBlogsAsync(search));
    }


    [HttpGet("paged")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] BlogFiltersDto filters)
    {
        return Ok(await _blogService.GetFilteredBlogsAsync(pageNumber, pageSize, filters));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _blogService.GetBlogByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] BlogPostDto blogPostDto)
    {
        return Ok(await _blogService.CreateBlogAsync(blogPostDto));
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromForm] BlogPutDto blogPutDto)
    {

        return Ok(await _blogService.UpdateBlogAsync(blogPutDto));
    }

    [HttpDelete("{id}")] 
    public async Task<IActionResult> Delete(Guid id)
    {

        return Ok(await _blogService.DeleteBlogAsync(id));
    }
}