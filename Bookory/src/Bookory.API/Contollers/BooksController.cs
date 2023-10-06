using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BooksController(IBookService bookService, IWebHostEnvironment webHostEnvironment)
    {
        _bookService = bookService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        return Ok(await _bookService.GetAllBooksAsync(search));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await _bookService.GetBookByIdAsync(id));
    }

    [HttpGet("pending-or-rejected")]
    public async Task<IActionResult> GetPendingOrRejectedBooks()
    {
        var books = await _bookService.GetBooksPendingApprovalOrRejectedAsync();
        return Ok(books);
    }

    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetByCompanyId(Guid companyId)
    {
        var books = await _bookService.GetBooksByCompanyIdAsync(companyId);
        return Ok(books);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] BookFiltersDto filters)
    {
        return Ok(await _bookService.GetPageOfBooksAsync( pageNumber, pageSize , filters));
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
        var updatedBook = new BookPutDto(id, bookPutDto.Title, bookPutDto.Images, bookPutDto.MainImageIndex, bookPutDto.Description, bookPutDto.Price, bookPutDto.DiscountPrice, bookPutDto.StockQuantity, bookPutDto.AuthorId, bookPutDto.GenreIds);
        var response = await _bookService.UpdateBookAsync(updatedBook);

        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _bookService.DeleteBookAsync(id);

        return StatusCode(response.StatusCode, response.Message);
    }


    [HttpPost("{bookId}/approve")]
    public async Task<IActionResult> ApproveBook(Guid bookId)
    {
        var response = await _bookService.ApproveOrRejectBookAsync(bookId, BookStatus.Approved);
        return Ok(response);
    }

    [HttpPost("{bookId}/reject")]
    public async Task<IActionResult> RejectBook(Guid bookId)
    {
        var response = await _bookService.ApproveOrRejectBookAsync(bookId, BookStatus.Rejected);
        return Ok(response);
    }

}
