using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class BlogRepository : Repository<Blog>, IBlogRepository
{
	public BlogRepository(AppDbContext context) : base(context)
	{

	}

}
