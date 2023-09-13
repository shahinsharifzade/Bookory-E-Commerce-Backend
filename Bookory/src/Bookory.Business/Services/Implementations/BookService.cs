using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Enums;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class BookService : IBookService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IBookRepository _bookRepository;
    private readonly ICompanyService _companyService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public BookService(IBookRepository bookRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IUserService userService, IHttpContextAccessor httpContextAccessor, ICompanyService companyService)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _companyService = companyService;
    }


    public async Task<List<BookGetResponseDto>> GetAllBooksAsync(string? search)
    {
        var books = await _bookRepository.GetFiltered(
        b => (string.IsNullOrEmpty(search) || b.Title.ToLower().Contains(search.Trim().ToLower())) && b.Status == BookStatus.Approved,
                includes).ToListAsync();

        if (books == null || books.Count == 0)
            throw new BookNotFoundException("No books were found matching the search criteria.");

        var bookDtos = _mapper.Map<List<BookGetResponseDto>>(books);
        return bookDtos;
    }

    public async Task<BookGetResponseDto> GetBookByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetSingleAsync(b => b.Status == BookStatus.Approved, includes);

        if (book is null)
            throw new BookNotFoundException($"The book with ID {id} was not found");

        var bookDto = _mapper.Map<BookGetResponseDto>(book);
        return bookDto;
    }

    public async Task<Book> GetBookAllDetailsByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id, includes);

        if (book is null)
            throw new BookNotFoundException($"No book with the specified ID ({id}) was found");

        return book;
    }

    public async Task<List<BookGetResponseDto>> GetBooksByCompanyIdAsync(Guid id)
    {
        var books = await _bookRepository.GetFiltered( b => b.CompanyId == id , includes).ToListAsync();

        if (books == null || books.Count == 0)
            throw new BookNotFoundException("No books were found matching the search criteria.");

        var bookDtos = _mapper.Map<List<BookGetResponseDto>>(books);
        return bookDtos;
    }

    public async Task<ResponseDto> CreateBookAsync(BookPostDto bookPostDto)
    {
        bool isExist = await _bookRepository.IsExistAsync(b => b.Title.ToLower().Trim() == bookPostDto.Title.ToLower().Trim());
        if (isExist)
            throw new BookAlreadyExistException($"A book with the title '{bookPostDto.Title}' already exists");

        var newBook = _mapper.Map<Book>(bookPostDto);

        var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var userDetail = await _userService.GetUserByUsernameAsync(username);

        if (userDetail.User.IsVendorRegistrationComplete == false) throw new Exception("Olmaz");

        if (userDetail.Role == Roles.Vendor.ToString())
        {
            var company = await _companyService.GetCompanyByUsernameAsync(username);

            newBook.Status = BookStatus.PendingApproval;
            newBook.CompanyId = userDetail.User.CompanyId;

            company.Books.Add(newBook);
        }

        await _bookRepository.CreateAsync(newBook);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "The book has been successfully created");
    }

    public async Task<ResponseDto> UpdateBookAsync(BookPutDto bookPutDto)
    {
        bool isExist = await _bookRepository.IsExistAsync(b => b.Title.ToLower().Trim() == bookPutDto.Title.ToLower().Trim() && b.Id != bookPutDto.Id);
        if (isExist)
            throw new BookAlreadyExistException($"A book with the title '{bookPutDto.Title}' already exists");

        var book = await _bookRepository.GetSingleAsync(b => b.Id == bookPutDto.Id, nameof(Book.Images));
        if (book is null)
            throw new BookNotFoundException($"No book found with the ID {bookPutDto.Id}");

        DeleteBookImages(bookPutDto, book);

        var updatedBook = _mapper.Map(bookPutDto, book);

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetUserByIdAsync(userId);
        if (user.Role == Roles.Vendor.ToString())
            updatedBook.Status = BookStatus.PendingApproval;

        _bookRepository.Update(updatedBook);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "The book was successfully updated");
    }

    public async Task<ResponseDto> DeleteBookAsync(Guid Id)
    {
        var book = await _bookRepository.GetSingleAsync(b => b.Id == Id);
        if (book is null)
            throw new BookNotFoundException($"The book with ID {Id} was not found");

        _bookRepository.SoftDelete(book);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, "The book has been successfully deleted");
    }

    public async Task<List<BookGetResponseDto>> GetBooksPendingApprovalOrRejectedAsync()
    {
        var books = await _bookRepository.GetFiltered(
            b => (b.Status == BookStatus.PendingApproval || b.Status == BookStatus.Rejected),
            includes).ToListAsync();

        var bookDtos = _mapper.Map<List<BookGetResponseDto>>(books);
        return bookDtos;
    }

    public async Task<ResponseDto> ApproveOrRejectBookAsync(Guid bookId, BookStatus status)
    {
        var book = await _bookRepository.GetByIdAsync(bookId, includes);

        if (book is null)
            throw new BookNotFoundException($"No book found with the ID {bookId}");

        if (status != BookStatus.Approved && status != BookStatus.Rejected)
            throw new ArgumentException("Invalid book status. Only 'Approved' or 'Rejected' are allowed");

        book.Status = status;

        var user = _userService.GetUserByIdAsync(book.Company.UserId);
        //user.g

        _bookRepository.Update(book);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.OK, $"The book has been {status.ToString().ToLower()} successfully");
    }

    public async Task<bool> BookIsExistAsync(Guid id)
    {
        bool isExist = await _bookRepository.IsExistAsync(b => b.Id == id);
        return isExist;
    }

    public async Task<Book> IncludeBookAsync(Guid id)
    {
        return (await _bookRepository.GetSingleAsync(b => b.Id == id,
                                                                    nameof(Book.Images),
                                                                    nameof(Book.Author),
                                                                    $"{nameof(Book.Author)}.{nameof(Author.Images)}",
                                                                    $"{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}"));
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



    private static readonly string[] includes = {
    nameof(Book.Images),
    nameof(Book.Author),
    nameof(Book.Company),
    $"{nameof(Book.Author)}.{nameof(Author.Images)}",
    $"{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}"};
}
