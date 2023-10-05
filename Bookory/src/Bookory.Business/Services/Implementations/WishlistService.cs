using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.WishlistDtos;
using Bookory.Business.Utilities.Exceptions.BookExceptions;
using Bookory.Business.Utilities.Exceptions.WishlistExceptions;
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
            WishlistGetResponseDto wishlistDto =  GetWishlistItemsFromCookie();
            return wishlistDto;
        }

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var wishlist = await _wishlistRepository.GetSingleAsync(wl => wl.User.Id == userId,
            nameof(Wishlist.Books),
            $"{nameof(Wishlist.Books)}.{nameof(Book.Images)}",
            $"{nameof(Wishlist.Books)}.{nameof(Book.Author)}",
            $"{nameof(Wishlist.Books)}.{nameof(Book.Author)}.{nameof(Author.Images)}",
            $"{nameof(Wishlist.Books)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}");

        if (wishlist is null)
            throw new WishlistItemNotFoundException("The wishlist is empty. No wishlist items exist.");

        return _mapper.Map<WishlistGetResponseDto>(wishlist);
    }

    public async Task<ResponseDto> AddItemToWishlist(WishlistPostDto wishlistPostDto)
    {
        var book = await _bookService.GetBookAllDetailsByIdAsync(wishlistPostDto.Id);
        if (book is null) throw new BookNotFoundException($"No book was found with ID: {wishlistPostDto.Id}");

        if (!isAuthenticated)
        {
            var response = await AddWishlistItemToCookieAsync(book);
            return new ResponseDto(response.StatusCode, response.Message);
        }

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wishlist = await _wishlistRepository.GetSingleAsync(wl => wl.UserId == userId, nameof(Wishlist.Books));

        var isExistItem = wishlist.Books.FirstOrDefault(wl => wl.Id == book.Id);
        if (isExistItem != null) throw new WishlistItemAlreadyExistException("The selected item already exists in your wishlist");

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

        return new ResponseDto((int)HttpStatusCode.OK, "Item successfully added to your wishlist.");
    }

    public async Task<ResponseDto> RemoveWishlistItem(Guid id)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var bookToDelete = await _bookService.GetBookByIdAsync(id);

        if (!isAuthenticated)
        {
            var response =  DeleteWishlistItemFromCookie(bookToDelete);
            return new ResponseDto(response.StatusCode, response.Message);
        }

        var wishlist = await _wishlistRepository.GetSingleAsync(w => w.UserId == userId, nameof(Wishlist.Books));
        if (wishlist == null)
            throw new WishlistItemNotFoundException("The wishlist is empty. No wishlist items exist.");

        var book = wishlist.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new WishlistItemNotFoundException($"No book found in the wishlist with ID {id}");

        wishlist.Books.Remove(book);
        _wishlistRepository.Update(wishlist);
        await _wishlistRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.OK, "The item has been successfully removed from your wishlist.");
    }

    public async Task<ResponseDto> TransferCookieWishlistToDatabaseAsync(string userId)
    {
        var cookieWishlist = GetWishlistItemsFromCookie();

        if (cookieWishlist.Books is null || !cookieWishlist.Books.Any())
            return new ResponseDto((int)HttpStatusCode.OK, "No items found in the cookie wishlist to transfer");

        Wishlist wishlist = await _wishlistRepository.GetSingleAsync(wl => wl.UserId == userId, nameof(Wishlist.Books));

        if (wishlist == null)
        {
            // Create a new wishlist if it doesn't exist for the user
            wishlist = new Wishlist
            {
                UserId = userId,
                Books = new List<Book>()
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

        _wishlistRepository.Update(wishlist); // Update the existing or newly created wishlist
        await _wishlistRepository.SaveAsync();
        ClearCookieBasket();

        return new ResponseDto((int)HttpStatusCode.OK, "Cookie wishlist successfully transferred to the database");
    }


    #region Cookie Wishlist Mehtods

    private WishlistGetResponseDto GetWishlistItemsFromCookie()
    {
        var cookie = _httpContextAccessor.HttpContext.Request.Cookies[COOKIE_WISHLIST_ITEM_KEY];
        List<string> wishlistIds;

        if (cookie != null)
            wishlistIds = JsonConvert.DeserializeObject<List<string>>(cookie);
        else
            wishlistIds = new List<string>();

        WishlistGetResponseDto wishlistDto = new WishlistGetResponseDto
        {
            Books = new List<BookGetResponseDto>()
        };

        foreach (var bookId in wishlistIds)
        {
            var book = _bookService.GetBookByIdAsync(Guid.Parse(bookId)).Result; 
            if (book != null)
            {
                var bookDto = _mapper.Map<BookGetResponseDto>(book);
                wishlistDto.Books.Add(bookDto);
            }
        }

        return wishlistDto;
    }


    private async Task<ResponseDto> AddWishlistItemToCookieAsync(Book book)
    {
        var cookie = _httpContextAccessor.HttpContext.Request.Cookies[COOKIE_WISHLIST_ITEM_KEY];
        List<string> wishlistIds;

        if (cookie != null)
            wishlistIds = JsonConvert.DeserializeObject<List<string>>(cookie);

        else
            wishlistIds = new List<string>();


        if (wishlistIds.Contains(book.Id.ToString()))
            throw new WishlistItemAlreadyExistException("The item already exists in the wishlist");

        wishlistIds.Add(book.Id.ToString());

        _httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_WISHLIST_ITEM_KEY, JsonConvert.SerializeObject(wishlistIds), new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.UtcNow.AddMonths(1) });

        return new ResponseDto((int)HttpStatusCode.Created, "The book has been successfully added.");
    }

    private ResponseDto DeleteWishlistItemFromCookie(BookGetResponseDto book)
    {
        var cookie = _httpContextAccessor.HttpContext.Request.Cookies[COOKIE_WISHLIST_ITEM_KEY];
        List<string> wishlistIds;

        if (cookie != null)
            wishlistIds = JsonConvert.DeserializeObject<List<string>>(cookie);
        else
            throw new WishlistItemNotFoundException($"No item found in the wishlist by ID {book.Id}");

        if (!wishlistIds.Contains(book.Id.ToString())) throw new WishlistItemNotFoundException($"No item found in the wishlist by ID {book.Id}");

        wishlistIds.Remove(book.Id.ToString());

        _httpContextAccessor.HttpContext.Response.Cookies.Append(COOKIE_WISHLIST_ITEM_KEY, JsonConvert.SerializeObject(wishlistIds), new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.None, Secure = true });

        return new ResponseDto((int)HttpStatusCode.OK, "The item has been successfully removed");
    }


    private void ClearCookieBasket()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(COOKIE_WISHLIST_ITEM_KEY, new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.None, Secure = true});
    }

    public async Task<ResponseDto> CheckItemExistsInWishlistAsync(Guid id)
    {
        if (!isAuthenticated)
        {
            var wishlistDto = GetWishlistItemsFromCookie();
            var isExist = wishlistDto.Books.Any(b => b.Id == id);

            if (!isExist) throw new WishlistItemNotFoundException("The wishlist is empty. No wishlist items exist");

            return new ResponseDto((int)HttpStatusCode.OK, $"Item found in the wishlist by ID {id}");
        }

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wishlist =await _wishlistRepository.GetFiltered(w => w.UserId == userId, nameof(Wishlist.Books)).ToListAsync();

        if(wishlist == null) throw new WishlistItemNotFoundException("The wishlist is empty. No wishlist items exist");

        return new ResponseDto((int)HttpStatusCode.OK, $"Item found in the wishlist by ID {id}");
    }

    #endregion
}
