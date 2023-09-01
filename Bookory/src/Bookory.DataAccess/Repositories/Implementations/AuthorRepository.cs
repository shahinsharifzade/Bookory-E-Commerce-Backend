using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;
namespace Bookory.DataAccess.Repositories.Implementations;

public class AuthorRepository : Repository<Author> , IAuthorRepository
{
	public AuthorRepository(AppDbContext context) : base(context)
	{
	}
}
