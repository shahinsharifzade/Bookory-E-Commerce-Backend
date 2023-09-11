using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.GenreDtos;
using Bookory.Business.Utilities.Exceptions.GenreExceptions;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;
    
    public GenreService(IGenreRepository genreRepository, IMapper mapper)
    {
        _genreRepository = genreRepository;
        _mapper = mapper;
    }

    public async Task<List<GenreGetResponeDto>> GetAllGenresAsync(string? search)
    {
        var genres = await _genreRepository.GetFiltered(
            g => string.IsNullOrEmpty(search) ? true : g.Name.ToLower().Contains(search.Trim().ToLower()), includes).ToListAsync();

        if (genres == null || genres.Count == 0)
            throw new GenreNotFoundException("No genres found matching the search criteria.");

        var genresDto = _mapper.Map<List<GenreGetResponeDto>>(genres);
        return genresDto;
    }

    public async Task<GenreGetResponeDto> GetGenreByIdAsync(Guid id)
    {
        var genre = await _genreRepository.GetSingleAsync(g => g.Id == id, includes);

        if (genre is null)
            throw new GenreNotFoundException($"Genre with the specified ID '{id}' was not found");

        var genreDto = _mapper.Map<GenreGetResponeDto>(genre);
        return genreDto;
    }

    public async Task<ResponseDto> CreateGenreAsync(GenrePostDto genrePostDto)
    {
        bool isExist = await _genreRepository.IsExistAsync(g => g.Name.ToLower().Trim() == genrePostDto.Name.ToLower().Trim());
        if (isExist)
            throw new GenreAlreadyExistException($"A genre with the title '{genrePostDto.Name}' already exists. Please choose a different title");

        var newGenre = _mapper.Map<Genre>(genrePostDto);

        await _genreRepository.CreateAsync(newGenre);
        await _genreRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "Genre successfully created");
    }

    public async Task<ResponseDto> UpdateGenreAsync(GenrePutDto genrePutDto)
    {
        bool isExist = await _genreRepository.IsExistAsync(g => g.Name.ToLower().Trim() == genrePutDto.Name.ToLower().Trim() && g.Id != genrePutDto.Id);
        if (isExist)
            throw new GenreAlreadyExistException($"A genre with the title '{genrePutDto.Name}' already exists");

        var genre = await _genreRepository.GetSingleAsync(g => g.Id == genrePutDto.Id);
        if (genre is null)
            throw new GenreNotFoundException($"Genre with ID '{genrePutDto.Id}' not found");

        var newGenre = _mapper.Map(genrePutDto, genre);
        _genreRepository.Update(newGenre);
        await _genreRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "Genre updated successfully");
    }

    public async Task<ResponseDto> DeleteGenreAsync(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre is null)
            throw new GenreNotFoundException($"No genre found with the ID: {id}");

        _genreRepository.SoftDelete(genre);
        await _genreRepository.SaveAsync();
        return new((int)HttpStatusCode.OK, "Genre successfully deleted");
    }

    private static readonly string[] includes = {
        $"{nameof(Genre.BookGenres)}.{nameof(BookGenre.Book)}.{nameof(Book.Images)}",
        $"{nameof(Genre.BookGenres)}.{nameof(BookGenre.Book)}.{nameof(Book.Author)}.{nameof(Author.Images)}"
    };
}
