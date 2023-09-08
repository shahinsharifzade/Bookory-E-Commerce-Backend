using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(AppDbContext context) : base(context)
    { 
    }
}
