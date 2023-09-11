using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Core.Models;

namespace Bookory.Business.Services.Interfaces;

public interface IBookService
{
    Task<List<BookGetResponseDto>> GetAllBooksAsync(string? search);
    Task<BookGetResponseDto> GetBookByIdAsync(Guid id);
    Task<ResponseDto> CreateBookAsync(BookPostDto bookPostDto);
    Task<ResponseDto> UpdateBookAsync(BookPutDto bookPutDto);
    Task<ResponseDto> DeleteBookAsync(Guid id);


    Task<Book> GetBookAllDetailsByIdAsync(Guid id);
    Task<Book> IncludeBookAsync(Guid id);
    Task<bool> BookIsExistAsync(Guid id);
}
