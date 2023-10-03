using Bookory.Business.Utilities.Enums;
using Bookory.Core.Models.Common;
namespace Bookory.Core.Models;

public class Book : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }

    public decimal Rating { get; set; }
    public int NumberOfRatings { get; set; }

    public int SoldQuantity { get; set; }

    public Guid AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public BookStatus Status { get; set; }
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }

    public ICollection<BookGenre> BookGenres { get; set; } = null!;
    public ICollection<BookImage> Images { get; set; } = null!;
    public ICollection<BasketItem>? BasketItems { get; set; } 
    public ICollection<Wishlist>? Wishlists { get; set; }
    public Book()
    {
        BookGenres = new List<BookGenre>();
        Images = new List<BookImage>();
        BasketItems = new List<BasketItem>();
        Wishlists = new List<Wishlist>();
    }
}
