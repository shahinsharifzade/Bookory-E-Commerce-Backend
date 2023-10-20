using Bookory.Core.Models.Common;
using Bookory.Core.Models.Identity;

namespace Bookory.Core.Models;

public class UserAddress : BaseEntity
{
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? Telephone { get; set; }
    public string Mobile { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public AppUser User { get; set; }

    public ICollection<OrderDetail>? OrderDetails { get; set; }

    public UserAddress()
    {

        OrderDetails = new List<OrderDetail>();
    }

}
