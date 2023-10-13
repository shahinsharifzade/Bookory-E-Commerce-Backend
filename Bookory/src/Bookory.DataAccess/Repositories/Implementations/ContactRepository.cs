using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.DataAccess.Repositories.Implementations;

public class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(AppDbContext context) : base(context)
    {

    }
}
