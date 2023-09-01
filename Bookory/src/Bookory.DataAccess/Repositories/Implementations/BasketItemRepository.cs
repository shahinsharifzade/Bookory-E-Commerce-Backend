using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;
namespace Bookory.DataAccess.Repositories.Implementations;

public class BasketItemRepository : Repository<BasketItem>, IBasketItemRepository
{
	public BasketItemRepository(AppDbContext context ) : base( context )
	{
	}
}
