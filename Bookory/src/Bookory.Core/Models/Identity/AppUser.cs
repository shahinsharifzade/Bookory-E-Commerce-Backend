using Microsoft.AspNetCore.Identity;
namespace Bookory.Core.Models.Identity;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public bool IsActive { get; set; }
    //public string? StripeTokenId { get; set; }

    public bool? IsVendorRegistrationComplete { get; set; }
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }

    public ICollection<ShoppingSession>? ShoppingSessions { get; set; }
    public ICollection<UserAddress>? UserAddresses { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public Wishlist Wishlist { get; set; }

    public AppUser()
    {
        ShoppingSessions = new List<ShoppingSession>();
        UserAddresses = new List<UserAddress>();
        OrderDetails = new List<OrderDetail>();
        Comments= new List<Comment>();
    }
}
