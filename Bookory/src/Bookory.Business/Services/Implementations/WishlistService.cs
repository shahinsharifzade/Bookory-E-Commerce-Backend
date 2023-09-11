using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.WishlistDtos;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class WishlistService : IWishlistService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly bool isAuthenticated;
    private readonly IMapper _mapper;

    private const string COOKIE_WISHLIST_ITEM_KEY = "mywishlistitemkey";

    public WishlistService(IMapper mapper, IWishlistRepository wishlistRepository, IBookService bookService, IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _mapper = mapper;
        _wishlistRepository = wishlistRepository;
        _bookService = bookService;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public async Task<WishlistGetResponseDto> GetAllWishlistAsync()
    {
        if (!isAuthenticated)
        {
            WishlistGetResponseDto wishlistDto = await GetWishlistItemsFromCookieAsync();
            return wishlistDto;
        }

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var wishlist = await _wishlistRepository.GetSingleAsync(wl => wl.User.Id == userId,
            nameof(Wishlist.Books),
            $"{nameof(Wishlist.Books)}.{nameof(Book.Images)}",
            $"{nameof(Wishlist.Books)}.{nameof(Book.Author)}",
            $"{nameof(Wishlist.Books)}.{nameof(Book.Author)}.{nameof(Author.Images)}",
            $"{nameof(Wishlist.Books)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}");

        return _mapper.Map<WishlistGetResponseDto>(wishlist);
    }

    public async Task<ResponseDto> AddItemToWishlist(Guid id)
    {
        var book = await _bookService.GetBookAllDetailsByIdAsync(id);
        if (book is null) throw new BookNotFoundException($"Book not found by Id: {id}");

        if (!isAuthenticated)
        {
            var response = await AddWishlistItemToCookieAsync(book);
            return new ResponseDto(response.StatusCode, response.Message);
        }

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wishlist = await _wishlistRepository.GetSingleAsync(wl => wl.UserId == userId, nameof(Wishlist.Books));

        var isExistItem = wishlist.Books.FirstOrDefault(wl => wl.Id == book.Id);
        if (isExistItem != null) throw new Exception("Item already Exist");

        if (wishlist is null)
        {
            var newWishlist = new Wishlist { UserId = userId };
            newWishlist.Books.Add(book);
            await _wishlistRepository.CreateAsync(newWishlist);
        }
        else
        {
            wishlist.Books.Add(book);
            _wishlistRepository.Update(wishlist);
        }
        await _wishlistRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "Success");
    }

    public async Task<ResponseDto> TransferCookieWishlistToDatabaseAsync(string userId)
    {
        var cookieWishlist = await GetWishlistItemsFromCookieAsync();

        if (cookieWishlist.Books is null || !cookieWishlist.Books.Any())
            return new ResponseDto((int)HttpStatusCode.OK, "No cookie wishlist items to transfer");

        //var newWishlist = _mapper.Map<Wishlist>(new WishlistGetResponseDto(Guid.NewGuid(), UserId: userId, Books: cookieWishlist.Books));
        Wishlist wishlist = await _wishlistRepository.GetSingleAsync(wl => wl.UserId == userId, nameof(Wishlist.Books));

        if (wishlist is null)
        {
            wishlist = new()
            {
                UserId = userId
            };
        }

        foreach (var cookieBook in cookieWishlist.Books)
        {
            var isExistItem = wishlist.Books.FirstOrDefault(wl => wl.Id == cookieBook.Id);
            if (isExistItem == null)
            {
                var book = await _bookService.GetBookAllDetailsByIdAsync(cookieBook.Id);
                wishlist.Books.Add(book);
            }
        }

        await _wishlistRepository.CreateAsync(wishlist);
        await _wishlistRepository.SaveAsync();
        ClearCookieBasket();

        return new ResponseDto((int)HttpStatusCode.OK, "Cookie wishlist successfully transferred to the database.");
    }

    public async Task<ResponseDto> RemoveWishlistItem(Guid id)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var bookToDelete = await _bookService.GetBookByIdAsync(id);

        if (!isAuthenticated)
        {
            var response = await DeleteWishlistItemFromCookie(bookToDelete);
            return new ResponseDto(response.StatusCode, response.Message);
        }

        var wishlist = await _wishlistRepository.GetSingleAsync(w => w.UserId == userId, nameof(Wishlist.Books));
        if (wishlist == null)
            throw new Exception("Wishlist not found");

        var book = wishlist.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new Exception("Book not found in wishlist by id " + id);

        wishlist.Books.Remove(book);
        _wishlistRepository.Update(wishlist);
        await _wishlistRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "Success");
    }



    #region Cookie Wishlist Mehtods

    private async Task<WishlistGetResponseDto> GetWishlistItemsFromCookieAsync()
    {
        List<BookGetResponseDto> bookGetResponseDtos = null;

        var cookie = _httpContextAccessor.HttpContext.Request.Cookies[COOKIE_WISHLIST_ITEM_KEY];
        if (cookie != null) bookGetResponseDtos = JsonConvert.DeserializeObject<List<BookGetResponseDto>>(cookie);

        WishlistGetResponseDto wishlistDto = new WishlistGetResponseDto
        {
            Books = new List<BookGetResponseDto>() // Initialize Books property
        };

        if (bookGetResponseDtos != null)
            foreach (var item in bookGetResponseDtos)
                wishlistDto.Books.Add(item);

        return wishlistDto;
    }

    private async Task<ResponseDto> AddWishlistItemToCookieAsync(Book book)
    {
        WishlistGetResponseDto wishlistDto = await GetWishlistItemsFromCookieAsync();

        var bookDto = _mapper.Map<BookGetResponseDto>(book);

        if (wishlistDto.Books != null)

            if (wishlistDto.Books.Any(ck => ck.Id == book.Id))
                throw new Exception("Book already exist");
            else
                wishlistDto.Books.Add(bookDto);

        else
            wishlistDto.Books.Add(bookDto);

        _httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_WISHLIST_ITEM_KEY, JsonConvert.SerializeObject(wishlistDto.Books));
        return new ResponseDto((int)HttpStatusCode.Created, "Book successfully added");
    }

    private async Task<ResponseDto> DeleteWishlistItemFromCookie(BookGetResponseDto book)
    {
        var wishlistItem = await GetWishlistItemsFromCookieAsync();
        if (!wishlistItem.Books.Any(wl => wl.Id == book.Id)) throw new Exception("Not found by id " + book.Id);

        var bookToDelete = wishlistItem.Books.FirstOrDefault(wl => wl.Id == book.Id);
        wishlistItem.Books.Remove(bookToDelete);

        _httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_WISHLIST_ITEM_KEY, JsonConvert.SerializeObject(wishlistItem.Books));
        return new ResponseDto((int)HttpStatusCode.OK, "Deleted");
    }

    private void ClearCookieBasket()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(COOKIE_WISHLIST_ITEM_KEY);
    }


    #endregion

    //private async Task IncludeBookToWishlistAsync(List<BookGetResponseDto> bookDto) 
    //{
    //    if (bookDto != null)
    //    {
    //        foreach (var book in bookDto)
    //        {

    //        }
    //    }
    //}
}
