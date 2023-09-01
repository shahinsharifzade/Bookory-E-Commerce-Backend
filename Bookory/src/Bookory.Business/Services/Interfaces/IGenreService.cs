using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.GenreDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IGenreService
{
    Task<List<GenreGetResponeDto>> GetAllGenresAsync(string? search);
    Task<GenreGetResponeDto> GetGenreByIdAsync(Guid id );
    Task<ResponseDto> CreateGenreAsync(GenrePostDto genrePostDto);
    Task<ResponseDto> UpdateGenreAsync(GenrePutDto genrePutDto);
    Task<ResponseDto> DeleteGenreAsync(Guid id);


}
