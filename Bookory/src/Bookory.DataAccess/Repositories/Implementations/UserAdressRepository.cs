using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class UserAdressRepository : Repository<UserAddress> , IUserAdressRepository
{
	public UserAdressRepository( AppDbContext context) : base(context)
	{
	}
}
