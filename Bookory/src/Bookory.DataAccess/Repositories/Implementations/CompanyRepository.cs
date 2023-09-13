using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
	public CompanyRepository(AppDbContext context) : base(context)
	{
	}
}
