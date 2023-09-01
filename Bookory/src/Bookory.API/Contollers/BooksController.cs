using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(  [FromQuery] string? search)
    {
        return Ok(await _bookService.GetAllBooksAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok( await _bookService.GetBookByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] BookPostDto bookPostDto)
    {
        var response = await _bookService.CreateBookAsync(bookPostDto);

        return StatusCode(response.StatusCode, response.Message); 
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromForm] BookPutDto bookPutDto)
    {
        var updatedBook = new BookPutDto(id, bookPutDto.Title, bookPutDto.Images, bookPutDto.MainImageIndex , bookPutDto.Description , bookPutDto.Price , bookPutDto.DiscountPrice , bookPutDto.AuthorId);
        var response = await _bookService.UpdateBookAsync(updatedBook);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpDelete("{id}")]
    public  async Task<IActionResult> Delete( Guid id)
    {
        var response = await _bookService.DeleteBookAsync(id);

        return StatusCode(response.StatusCode, response.Message);
    }

}
