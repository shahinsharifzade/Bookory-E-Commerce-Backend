using Bookory.Core.Models;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Bookory.DataAccess.Initalizers;

public class DbContextInitalizer
{
    private readonly AppDbContext _context;

    public DbContextInitalizer(AppDbContext context)
    {
        _context = context;
    }


    public async Task InitDatabaseAsync()
    {

      

        await _context.Database.MigrateAsync();
    }
}
