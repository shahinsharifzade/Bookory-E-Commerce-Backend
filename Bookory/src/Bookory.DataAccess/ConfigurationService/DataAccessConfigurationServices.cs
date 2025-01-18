using Bookory.Core.Models.Common;
using Bookory.DataAccess.Initalizers;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Bookory.DataAccess.Repositories.Implementations;
using Bookory.DataAccess.Repositories.Interfaces;
using ECommerce.DataAccessLayer.Persistance.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookory.DataAccess.ConfigurationService;

public static class DataAccessConfigurationServices
{
    public static IServiceCollection AddRepositoriesService(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IBasketItemRepository, BasketItemRepository>();
        services.AddScoped<IShoppingSessionRepository, ShoppingSessionRepository>();
        services.AddScoped<IUserAdressRepository, UserAdressRepository>();

        services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();


        services.AddScoped<DbContextInitalizer>();

        return services;
    }

    public static IServiceCollection AddDatabaseSevice(this IServiceCollection services , IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });
        services.AddScoped<BaseEntityInterceptor>();
        return services;
    }
}
