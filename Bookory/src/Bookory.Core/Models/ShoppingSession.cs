using Bookory.Core.Models.Common;
using Bookory.Core.Models.Identity;

namespace Bookory.Core.Models;

public class ShoppingSession : BaseEntity 
{
    public decimal TotalPrice { get; set; }
    public bool IsOrdered { get; set; } = false;

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public ICollection<BasketItem> BasketItems { get; set; } = null!;
    public ShoppingSession()
    {
        BasketItems = new List<BasketItem>();
    }
}
