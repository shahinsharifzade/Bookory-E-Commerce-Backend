using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;
namespace Bookory.DataAccess.Repositories.Implementations;

public class ShoppingSessionRepository : Repository<ShoppingSession>, IShoppingSessionRepository
{
	public ShoppingSessionRepository(AppDbContext context) : base(context)
	{
	}
}
