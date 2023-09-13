using Bookory.Business.Utilities.Enums;
using Bookory.Core.Models.Common;
using Bookory.Core.Models.Identity;

namespace Bookory.Core.Models;

public class Company : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string BannerImage { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string? Address { get; set; } 
    public decimal? Rating { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public CompanyStatus? Status { get; set; }
    public ICollection<Book> Books { get; set; } 
    public Company()
    {
        Books= new List<Book>();
    }
}
