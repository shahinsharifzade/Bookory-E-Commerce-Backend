using Microsoft.AspNetCore.Identity;
namespace Bookory.Core.Models.Identity;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public bool IsActive { get; set; }
    public ICollection<ShoppingSession>? ShoppingSessions { get; set; }
    public ICollection<UserAddress>? UserAddresses { get; set; }

    public AppUser()
    {
        ShoppingSessions = new List<ShoppingSession>();
        UserAddresses = new List<UserAddress>();
    }

}
