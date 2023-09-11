using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class WishlistRepository : Repository<Wishlist> , IWishlistRepository
{
	public WishlistRepository(AppDbContext context) : base(context)
	{
	}
}
