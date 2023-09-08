using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    public BookService(IBookRepository bookRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }


    public async Task<List<BookGetResponseDto>> GetAllBooksAsync(string? search)
    {
        var books = await _bookRepository.GetFiltered(g => string.IsNullOrEmpty(search) ? true : g.Title.ToLower().Contains(search.Trim().ToLower()),
            nameof(Book.Images),
            nameof(Book.Author),
            $"{nameof(Book.Author)}.{nameof(Author.Images)}",
            $"{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}").ToListAsync();

        var bookDtos = _mapper.Map<List<BookGetResponseDto>>(books);
        return bookDtos;
    }

    public async Task<BookGetResponseDto> GetBookByIdAsync(Guid id) 
    {
        var book = await _bookRepository.GetByIdAsync(id,
            nameof(Book.Author),
            nameof(Book.Images),
            $"{nameof(Book.Author)}.{nameof(Author.Images)}",
            $"{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}");

        if (book is null)
            throw new BookNotFoundException($"Book not found by Id {id}");

        var bookDto = _mapper.Map<BookGetResponseDto>(book);
        return bookDto;
    }

    public async Task<ResponseDto> CreateBookAsync(BookPostDto bookPostDto)
    {
        bool isExist = await _bookRepository.IsExistAsync(b => b.Title.ToLower().Trim() == bookPostDto.Title.ToLower().Trim());
        if (isExist) throw new BookAlreadyExistException($"Book already exist with title : {bookPostDto.Title}");

        var newBook = _mapper.Map<Book>(bookPostDto);

        await _bookRepository.CreateAsync(newBook);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "Book successfully created");
    }

    public async Task<ResponseDto> UpdateBookAsync(BookPutDto bookPutDto)
    {
        bool isExist = await _bookRepository.IsExistAsync(b => b.Title.ToLower().Trim() == bookPutDto.Title.ToLower().Trim() && b.Id != bookPutDto.Id);
        if (isExist) throw new BookAlreadyExistException($"Book already exist with title : {bookPutDto.Title}");

        var book = await _bookRepository.GetSingleAsync(b => b.Id == bookPutDto.Id, nameof(Book.Images));
        if (book is null) throw new BookNotFoundException($"Book not found by Id {bookPutDto.Id}");

        DeleteBookImages(bookPutDto, book);

        Book updatedBook = _mapper.Map(bookPutDto, book);

        _bookRepository.Update(updatedBook);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "Book successfully updated");
    }

    private void DeleteBookImages(BookPutDto bookPutDto, Book? book)
    {
        if (bookPutDto.Images != null)
        {
            foreach (var image in book.Images)
            {
                FileHelper.DeleteFile(new string[] { _webHostEnvironment.WebRootPath, "assets", "images", "books", image.Image });
            }
        }
    }

    public async Task<ResponseDto> DeleteBookAsync(Guid Id)
    {
        var book = await _bookRepository.GetSingleAsync(b => b.Id == Id);
        if (book is null) throw new BookNotFoundException($"Book not found by Id {Id}");

        _bookRepository.SoftDelete(book);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "Book successfully delete");
    }

    public async Task<bool> BookIsExistAsync(Guid id)
    {
        bool isExist = await _bookRepository.IsExistAsync(b => b.Id == id);
        return isExist;
    }
}
