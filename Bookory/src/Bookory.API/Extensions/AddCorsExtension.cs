using Microsoft.AspNetCore.Authentication.Cookies;

namespace Bookory.API.Extensions;
public static class AddCorsExtension
{
    public static IServiceCollection AddCorsService(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("https://bookory-e-commerce-frontend.vercel.app")  // Sadece frontend URL'si
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();  // CORS ile kimlik doğrulama (credentials) kullanımını sağlamak
            });
        });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
            });

        return services;
    }
}


//using Microsoft.AspNetCore.Authentication.Cookies;

//namespace Bookory.API.Extensions;

//public static class AddCorsExtension
//{
//    public static IServiceCollection AddCorsService(this IServiceCollection services)
//    {
//        services.AddCors(options =>
//        {
//            options.AddDefaultPolicy(builder =>
//            {
//                builder
//                    .SetIsOriginAllowed(origin => true) // Geliştirme aşamasında güvenli olmayan bir yaklaşım
//                                                        //.WithOrigins(
//                                                        //    "http://localhost:3000",
//                                                        //    "https://localhost:3000",
//                                                        //    "http://shahin20sh-001-site1.qtempurl.com",
//                                                        //    "https://shahin20sh-001-site1.qtempurl.com"
//                                                        //)
//                    .AllowAnyMethod()
//                    .AllowAnyHeader()
//                    .AllowCredentials();
//            });
//        });

//        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//            .AddCookie(options =>
//            {
//                options.Cookie.SameSite = SameSiteMode.None;
//                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//                options.Cookie.HttpOnly = true;
//            });

//        return services;
//    }
//}



// using Microsoft.AspNetCore.Authentication.Cookies;

// namespace Bookory.API.Extensions;

// public static class AddCorsExtension
// {
//     public static IServiceCollection AddCorsService(this IServiceCollection services)
//     {
//         services.AddCors(options =>
//         {
//             options.AddDefaultPolicy(builder =>
//             {
//                 builder.WithOrigins("http://localhost:3000", "https://localhost:3000", "https://bookory-e-commerce-frontend.vercel.app/","https://bookory-e-commerce-backend.onrender.com/api")
//                     .AllowAnyHeader()
//                     .AllowAnyMethod()
//                     .AllowCredentials();
//             });
//         });

//         services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//         .AddCookie(options =>
//         {
//             options.Cookie.SameSite = SameSiteMode.None;
//             options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//             options.Cookie.HttpOnly = false;
//         });

//         return services;
//     }
// }
