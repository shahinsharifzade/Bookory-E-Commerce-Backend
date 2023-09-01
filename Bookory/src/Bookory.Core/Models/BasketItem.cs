using Bookory.Core.Models.Common;
namespace Bookory.Core.Models;

public class BasketItem : BaseEntity
{
    public int Quantity { get; set; }
    public decimal Price { get; set; } //Unit price
    public Guid SessionId { get; set; }
    public ShoppingSession ShoppingSession { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
