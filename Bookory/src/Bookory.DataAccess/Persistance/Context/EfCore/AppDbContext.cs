using Bookory.Core.Models;
using Bookory.Core.Models.Common;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bookory.DataAccess.Persistance.Context.EfCore;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<AuthorImage> AuthorImages { get; set; } = null!;

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<BookImage> BookImages { get; set; } = null!;

    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<BookGenre> BookGenres { get; set; } = null!;

    public DbSet<Blog> Blogs { get; set; } = null!;

    public DbSet<BasketItem> BasketItems { get; set; } = null!;
    public DbSet<ShoppingSession> ShoppingSessions { get; set; } = null!;
    public DbSet<UserAddress> UserAddresses  { get; set; } = null!;

    public DbSet<PaymentDetail>  PaymentDetails { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Author>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Genre>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<BookGenre>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Blog>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<AuthorImage>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<BookImage>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<BasketItem>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ShoppingSession>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<UserAddress>().HasQueryFilter(x => !x.IsDeleted);


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var item in entries)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    item.Entity.CreatedAt = DateTime.UtcNow;
                    item.Entity.ModifiedAt = DateTime.UtcNow;
                    //item.Entity.CreateBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                    break;

                case EntityState.Modified:
                    item.Entity.ModifiedAt = DateTime.UtcNow;
                    //item.Entity.ModifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
