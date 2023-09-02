using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.Email;
using Bookory.Business.Utilities.Security.JWT.Implementation;
using Bookory.Business.Utilities.Security.JWT.Interface;

namespace Bookory.API.Extensions;

public static class AddServiceExtension
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IGenreService , GenreService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenHelper, JWTHelper>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IBasketService, BasketService>();

        services.AddHttpContextAccessor();
        services.AddTransient<IMailService, MailService>();
        return services;
    }
}
