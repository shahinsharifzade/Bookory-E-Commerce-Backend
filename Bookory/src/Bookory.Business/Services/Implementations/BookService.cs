using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.MailDtos;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Business.Utilities.Email;
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
    private readonly IBasketItemService _basketItemService;
    private readonly IBookRepository _bookRepository;
    private readonly ICompanyService _companyService;
    private readonly IAuthorService _authorService;
    private readonly IGenreService _genreService;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;

    public BookService(IBookRepository bookRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IUserService userService, IHttpContextAccessor httpContextAccessor, ICompanyService companyService, IMailService mailService, IBasketItemService basketItemService, IGenreService genreService, IAuthorService authorService)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _companyService = companyService;
        _mailService = mailService;
        _basketItemService = basketItemService;
        _genreService = genreService;
        _authorService = authorService;
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
        var book = await _bookRepository.GetSingleAsync(b => b.Id == id && b.Status == BookStatus.Approved, includes);

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
        var books = await _bookRepository.GetFiltered(b => b.CompanyId == id, includes).ToListAsync();

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

        await _authorService.GetAuthorByIdAsync(bookPostDto.AuthorId);
        foreach (var genreId in bookPostDto.GenreIds)
            await _genreService.GetGenreByIdAsync(genreId);

        var newBook = _mapper.Map<Book>(bookPostDto);

        var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var userDetail = await _userService.GetUserByUsernameAsync(username);

        if (userDetail.User.IsVendorRegistrationComplete == false) throw new BookCreationException("Failed to create a new book because the vendor's registration is not complete.");

        if (userDetail.Role == Roles.Vendor.ToString())
        {
            newBook.Status = BookStatus.PendingApproval;
            newBook.CompanyId = userDetail.User.CompanyId;

            var company = await _companyService.GetCompanyByUsernameAsync(username);
            company.Books.Add(newBook);
        }

        await _bookRepository.CreateAsync(newBook);
        await _bookRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "The book has been successfully created");
    }

    public async Task<ResponseDto> UpdateBookAsync(BookPutDto bookPutDto)
    {
        var book = await _bookRepository.GetSingleAsync(b => b.Id == bookPutDto.Id, nameof(Book.Images));
        if (book is null)
            throw new BookNotFoundException($"No book found with the ID {bookPutDto.Id}");

        bool isExist = await _bookRepository.IsExistAsync(b => b.Title.ToLower().Trim() == bookPutDto.Title.ToLower().Trim() && b.Id != bookPutDto.Id);
        if (isExist)
            throw new BookAlreadyExistException($"A book with the title '{bookPutDto.Title}' already exists");

        await _authorService.GetAuthorByIdAsync(bookPutDto.AuthorId);
        foreach (var genreId in bookPutDto.GenreIds)
            await _genreService.GetGenreByIdAsync(genreId);

        DeleteBookImages(bookPutDto, book);

        var updatedBook = _mapper.Map(bookPutDto, book);

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetUserByIdAsync(userId);

        foreach (var basketBookItem in updatedBook.BasketItems)
        {
            basketBookItem.Price = bookPutDto.Price;
            await _basketItemService.UpdateBasketItemAsync(basketBookItem);
        }

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

    public async Task<BookPageResponseDto> GetPageOfBooksAsync(int pageNumber, int pageSize, BookFiltersDto filters)
    {
        var booksQuery = _bookRepository.GetFiltered(b => b.Status == BookStatus.Approved, includes);
        booksQuery = booksQuery.OrderByDescending(b => b.Rating);

        if (filters.Authors != null && filters.Authors.Any())
            booksQuery = booksQuery.Where(b => filters.Authors.Any(a => a == b.AuthorId));

        if (filters.Genres != null && filters.Genres.Any())
            booksQuery = booksQuery.Where(b => b.BookGenres.Any(bg => filters.Genres.Contains(bg.Genre.Id)));

        if (filters.MinPrice != null && filters.MaxPrice != null)
            booksQuery = booksQuery.Where(b => (b.Price > filters.MinPrice && b.Price < filters.MaxPrice));

        if (filters.Rating != 0 && filters.Rating != null)
            booksQuery = booksQuery.Where(b => b.Rating == filters.Rating);

        if (filters.SortBy != null)
        {
            switch (filters.SortBy)
            {
                case "priceLowToHigh":
                    booksQuery = booksQuery.OrderBy(b => b.Price);
                    break;
                case "priceHighToLow":
                    booksQuery = booksQuery.OrderByDescending(b => b.Price);
                    break;
                case "averageRating":
                    booksQuery = booksQuery.OrderByDescending(b => b.Rating);
                    break;
                case "newest":
                    booksQuery = booksQuery.OrderByDescending(b => b.CreatedAt);
                    break;
                case "popularity":
                    booksQuery = booksQuery.OrderByDescending(b => b.SoldQuantity);
                    break;
                default:
                    booksQuery = booksQuery.OrderByDescending(b => b.Rating);
                    break;
            }
        }

        decimal totalCount = await booksQuery.CountAsync();

        if (pageSize != 0)
        {
            totalCount = Math.Ceiling((decimal)await booksQuery.CountAsync() / pageSize);
        }

        int itemsToSkip = (pageNumber - 1) * pageSize;
        booksQuery = booksQuery.Skip(itemsToSkip).Take(pageSize);

        var books = await booksQuery.ToListAsync();

        if (books is null || books.Count == 0)
            throw new BookNotFoundException("No books were found matching the provided criteria.");

        var bookGetResponseDto = _mapper.Map<List<BookGetResponseDto>>(books);

        BookPageResponseDto booksDtos = new(bookGetResponseDto, totalCount);

        return booksDtos;
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

        //Mail
        if (book.Company != null)
        {
            var user = await _userService.GetUserByIdAsync(book.Company.UserId);
            await SendStatusInformationMailAsync(status, book, user);
        }

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

    public async Task UpdateBookByEntityAsync(Book book)
    {
        _bookRepository.Update(book);
        await _bookRepository.SaveAsync();
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

    private async Task SendStatusInformationMailAsync(BookStatus status, Book? book, UserRoleGetResponseDto user)
    {
        string emailSubject = $"Book {status} Notification";
        string emailBody = $"Dear {user.User.UserName},\n\n" +
               $"We want to inform you that the book with the following details has been {status.ToString().ToLower()}:\n\n" +
               $"Title: {book.Title}\n" +
               $"Author: {book.Author.Name}\n" +
               $"Price: {book.Price}\n\n" +
               "Thank you for using our service!\n\n" +
               "Sincerely,\n" +
               "Your Bookstore Team";

        MailRequestDto mailRequestDto = new(
        user.User.Email,
        emailSubject,
        emailBody,
        null);

        await _mailService.SendEmailAsync(mailRequestDto);
    }

    public void UpdateBookByEntity(Book book)
    {
        _bookRepository.Update(book);
    }

    private static readonly string[] includes = {
    nameof(Book.BasketItems),
    nameof(Book.Images),
    nameof(Book.Author),
    nameof(Book.Company),
     $"{nameof(Book.Company)}.{nameof(Company.User)}",
    $"{nameof(Book.Author)}.{nameof(Author.Images)}",
    $"{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}"};
}
