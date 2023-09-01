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
        var genres = await _genreRepository.GetFiltered(g => string.IsNullOrEmpty(search) ? true : g.Name.ToLower().Contains(search.Trim().ToLower()),
        $"{nameof(Genre.BookGenres)}.{nameof(BookGenre.Book)}.{nameof(Book.Images)}",
        $"{nameof(Genre.BookGenres)}.{nameof(BookGenre.Book)}.{nameof(Book.Author)}.{nameof(Author.Images)}").ToListAsync();

        var genresDto = _mapper.Map<List<GenreGetResponeDto>>(genres);
        return genresDto;
    }

    public async Task<GenreGetResponeDto> GetGenreByIdAsync(Guid id)
    {
        var genre = await _genreRepository.GetSingleAsync(g => g.Id == id ,
        $"{nameof(Genre.BookGenres)}.{nameof(BookGenre.Book)}.{nameof(Book.Images)}",
        $"{nameof(Genre.BookGenres)}.{nameof(BookGenre.Book)}.{nameof(Book.Author)}.{nameof(Author.Images)}");

        if (genre is null) throw new GenreNotFoundException($"Genre not found with Id : {id}");

        var genreDto = _mapper.Map<GenreGetResponeDto>(genre);
        return genreDto;
    }

    public async Task<ResponseDto> CreateGenreAsync(GenrePostDto genrePostDto)
    {
        bool isExist = await _genreRepository.IsExistAsync(g => g.Name.ToLower().Trim() == genrePostDto.Name.ToLower().Trim());
        if (isExist) throw new GenreAlreadyExistException($"Genre already exist with title : {genrePostDto.Name}");

        var newGenre = _mapper.Map<Genre>(genrePostDto);

        await _genreRepository.CreateAsync(newGenre);
        await _genreRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "Genre successfully created");
    }

    public async Task<ResponseDto> UpdateGenreAsync(GenrePutDto genrePutDto)
    {
        bool isExist = await _genreRepository.IsExistAsync(g => g.Name.ToLower().Trim() == genrePutDto.Name.ToLower().Trim() && g.Id != genrePutDto.Id);
        if (isExist) throw new GenreAlreadyExistException($"Genre already exist with title : {genrePutDto.Name}");

        var genre = await _genreRepository.GetSingleAsync(g => g.Id == genrePutDto.Id);
        if (genre is null) throw new GenreNotFoundException($"Genre not found with Id : {genrePutDto.Id}");


        var newGenre = _mapper.Map(genrePutDto, genre);
        _genreRepository.Update(newGenre);
        await _genreRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "Genre successfully updated");
    }

    public async Task<ResponseDto> DeleteGenreAsync(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre is null) throw new GenreNotFoundException($"Genre not found with Id : {id}");

        _genreRepository.SoftDelete(genre);
        await _genreRepository.SaveAsync();
        return new((int)HttpStatusCode.OK, "Book successfully delete");
    }


}
