using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Exceptions.AuthorExceptions;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    public AuthorService(IAuthorRepository authorRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<AuthorGetResponseDto>> GetAllAuthorsAsync(string? search)
    {
        var authors = await _authorRepository.GetFiltered(g => string.IsNullOrEmpty(search) ? true : g.Name.ToLower().Contains(search.Trim().ToLower()),includes).ToListAsync();

        if (authors is null || authors.Count == 0)
            throw new AuthorNotFoundException("No authors were found matching the provided criteria.");

        var authorDtos = _mapper.Map<List<AuthorGetResponseDto>>(authors);
        return authorDtos;
    }

    public async Task<AuthorGetResponseDto> GetAuthorByIdAsync(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id,includes);

        if (author is null)
            throw new AuthorNotFoundException($"Author with ID {id} not found.");

        var authorDto = _mapper.Map<AuthorGetResponseDto>(author);
        return authorDto;
    }

    public async Task<ResponseDto> CreateAuthorAsync(AuthorPostDto authorPostDto)
    {
        bool isExist = await _authorRepository.IsExistAsync(a => a.Name.ToLower().Trim() == authorPostDto.Name.ToLower().Trim());
        if (isExist) throw new AuthorAlreadyExistException("An author with the same name already exists.");

        Author newAuthor = _mapper.Map<Author>(authorPostDto);

        await _authorRepository.CreateAsync(newAuthor);
        await _authorRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "Author has been successfully created");
    }

    public async Task<ResponseDto> UpdateAuthorAsync(AuthorPutDto authorPutDto)
    {
        bool isExist = await _authorRepository.IsExistAsync(b => b.Name.ToLower().Trim() == authorPutDto.Name.ToLower().Trim() && b.Id != authorPutDto.Id);
        if (isExist) throw new AuthorAlreadyExistException($"An author with the name '{authorPutDto.Name}' already exists.");

        var author = await _authorRepository.GetSingleAsync(b => b.Id == authorPutDto.Id, nameof(Author.Images));
        if (author is null) throw new AuthorNotFoundException($"Author not found with ID {authorPutDto.Id}");

        if (authorPutDto.Images != null)
            foreach (var image in author.Images)
            {
                FileHelper.DeleteFile(new string[] { _webHostEnvironment.WebRootPath, "assets", "images", "authors", image.Image });
            }

        var updatedAuthor = _mapper.Map(authorPutDto, author);

        _authorRepository.Update(updatedAuthor);
        await _authorRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "Author has been successfully updated");
    }

    public async Task<ResponseDto> DeleteAuthorAsync(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author is null)
            throw new AuthorNotFoundException($"No author found with ID {id}");

        _authorRepository.SoftDelete(author);
        await _authorRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "Author has been successfully deleted");
    }

    private static readonly string[] includes = {
    nameof(Author.Images),
    nameof(Author.Books),
    $"{nameof(Author.Books)}.{nameof(Book.Images)}",
    $"{nameof(Author.Books)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}"};
}
