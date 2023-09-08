using Bookory.Business.Services.Implementations;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.Email;
using Bookory.Business.Utilities.Security.JWT.Implementation;
using Bookory.Business.Utilities.Security.JWT.Interface;
using Bookory.Core.Models;
using Bookory.Core.Models.Stripe;
using Bookory.DataAccess.Repositories.Implementations;
using Stripe;

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
        services.AddScoped<IUserAddressService, UserAddressService>();

        services.AddScoped<IShoppingSessionService, ShoppingSessionService>();
        services.AddScoped<IBasketItemService, BasketItemService>();
        services.AddScoped<IOrderService, Business.Services.Implementations.OrderService>();
        services.AddScoped<IPaymentDetailService, PaymentDetailService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();

        services.AddScoped<IStripeService, StripeService>();

        services.AddScoped<StripeSettings>();
        
        services.AddHttpContextAccessor();
        services.AddTransient<IMailService, MailService>();
        return services;
    }
}
