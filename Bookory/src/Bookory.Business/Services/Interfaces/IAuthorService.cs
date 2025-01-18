using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.DTOs.Common;

namespace Bookory.Business.Services.Interfaces;

public interface IAuthorService
{
    Task<List<AuthorGetResponseDto>> GetAllAuthorsAsync(string? search);
    Task<AuthorGetResponseDto> GetAuthorByIdAsync(Guid id);
    Task<AuthorPageResponseDto> GetPageOfAuthorsAsync(int pageNumber, int pageSize);

    Task<ResponseDto> CreateAuthorAsync(AuthorPostDto authorPostDto);
    Task<ResponseDto> UpdateAuthorAsync(AuthorPutDto authorPutDto);
    Task<ResponseDto> DeleteAuthorAsync(Guid Id);
}
