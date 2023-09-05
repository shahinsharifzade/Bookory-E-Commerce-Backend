using Bookory.DataAccess.Repositories.Implementations;
using Bookory.DataAccess.Repositories.Interfaces;

namespace Bookory.API.Extensions;

public static class AddRepositoriesExtension
{
    public static IServiceCollection AddRepositoriesService(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IBasketItemRepository, BasketItemRepository>();
        services.AddScoped<IShoppingSessionRepository, ShoppingSessionRepository>();
        services.AddScoped<IUserAdressRepository, UserAdressRepository>();

        return services;
    }
}
