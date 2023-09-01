using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;

namespace Bookory.Business.Services.Interfaces;

public interface IBookService
{
    Task<List<BookGetResponseDto>> GetAllBooksAsync(string? search);
    Task<BookGetResponseDto> GetBookByIdAsync(Guid Id);
    Task<ResponseDto> CreateBookAsync(BookPostDto bookPostDto);
    Task<ResponseDto> UpdateBookAsync(BookPutDto bookPutDto);
    Task<ResponseDto> DeleteBookAsync(Guid Id);
}
