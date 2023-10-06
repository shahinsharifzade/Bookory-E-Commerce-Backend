using Bookory.Core.Models.Common;
using Bookory.Core.Models.Identity;

namespace Bookory.Core.Models;

public class Wishlist : BaseEntity
{
    public string? UserId { get; set; }
    public AppUser User { get; set; }

    public ICollection<Book> Books { get; set; }
        
    public Wishlist()
    {
        Books = new List<Book>();
    }
}
