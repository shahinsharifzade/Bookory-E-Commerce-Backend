using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.Email;
using Bookory.Business.Utilities.Email.Settings;
using Bookory.Business.Utilities.Security.JWT.Implementation;
using Bookory.Business.Utilities.Security.JWT.Interface;
using Bookory.Core.Models.Stripe;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using System.Reflection;

namespace Bookory.Business.ConfiguratoinService;

public static class BusinessConfigurationServices
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenHelper, JWTHelper>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IUserAddressService, UserAddressService>();

        services.AddScoped<IShoppingSessionService, ShoppingSessionService>();
        services.AddScoped<IBasketItemService, BasketItemService>();
        services.AddScoped<IOrderService, Business.Services.Implementations.OrderService>();
        services.AddScoped<IPaymentDetailService, PaymentDetailService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<IStripeService, StripeService>();

        services.AddScoped<StripeSettings>();

        services.AddHttpContextAccessor();
        services.AddTransient<IMailService, MailService>();
        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true).AddFluentValidationClientsideAdapters();

        return services;
    }

    public static IServiceCollection AddStripeServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
        StripeConfiguration.ApiKey = configuration.GetSection("Stripe:SecretKey").Value;

        return services;
    }
}
