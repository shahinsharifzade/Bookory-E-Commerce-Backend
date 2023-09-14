using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Enums;
using Bookory.Core.Models;

namespace Bookory.Business.Services.Interfaces;

public interface IBookService
{
    Task<List<BookGetResponseDto>> GetAllBooksAsync(string? search);
    Task<BookGetResponseDto> GetBookByIdAsync(Guid id);
    Task<Book> GetBookAllDetailsByIdAsync(Guid id);

    Task<ResponseDto> CreateBookAsync(BookPostDto bookPostDto);
    Task<ResponseDto> UpdateBookAsync(BookPutDto bookPutDto);
    Task<ResponseDto> DeleteBookAsync(Guid id);
    Task UpdateBookByEntityAsync(Book book);

    Task<List<BookGetResponseDto>> GetBooksByCompanyIdAsync(Guid id);
    Task<ResponseDto> ApproveOrRejectBookAsync(Guid bookId, BookStatus status);
    Task<List<BookGetResponseDto>> GetBooksPendingApprovalOrRejectedAsync();

    Task<Book> IncludeBookAsync(Guid id);
    Task<bool> BookIsExistAsync(Guid id);
}
