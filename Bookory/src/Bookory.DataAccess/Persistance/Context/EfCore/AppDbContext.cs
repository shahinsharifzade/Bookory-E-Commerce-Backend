using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using ECommerce.DataAccessLayer.Persistance.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bookory.DataAccess.Persistance.Context.EfCore;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly BaseEntityInterceptor _baseEntityInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor, BaseEntityInterceptor baseEntityInterceptor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _baseEntityInterceptor = baseEntityInterceptor;
    }

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<AuthorImage> AuthorImages { get; set; } = null!;

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<BookImage> BookImages { get; set; } = null!;

    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<BookGenre> BookGenres { get; set; } = null!;

    public DbSet<Blog> Blogs { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<BasketItem> BasketItems { get; set; } = null!;
    public DbSet<ShoppingSession> ShoppingSessions { get; set; } = null!;
    public DbSet<UserAddress> UserAddresses  { get; set; } = null!;

    public DbSet<PaymentDetail>  PaymentDetails { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Wishlist> Wishlists { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Company> Companies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Author>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Genre>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<BookGenre>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Blog>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<AuthorImage>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<BookImage>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<BasketItem>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ShoppingSession>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<UserAddress>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Wishlist>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Company>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Contact>().HasQueryFilter(x => !x.IsDeleted);


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_baseEntityInterceptor);
        base.OnConfiguring(optionsBuilder); 
    }
}
