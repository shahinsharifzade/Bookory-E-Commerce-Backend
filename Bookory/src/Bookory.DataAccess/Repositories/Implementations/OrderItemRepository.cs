using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class OrderItemRepository : Repository<OrderItem> , IOrderItemRepository
{
	public OrderItemRepository(AppDbContext context ) : base( context )
	{

	}
}
