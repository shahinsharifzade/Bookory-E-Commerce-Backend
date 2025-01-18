using Microsoft.OpenApi.Models;

namespace Bookory.API.Extensions;

public static class AddSwaggerGenExtension
{
    public static IServiceCollection AddSwaggerGenService(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Bookory API",
                Version = "v1",
                Description = "API documentation for the Bookory application"
            });

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

            // Server URL'sini ortama göre dinamik olarak ekleyelim
            if (!Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Development", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                option.AddServer(new OpenApiServer { Url = "http://shahin20sh-001-site1.qtempurl.com" });
            }
        });

        return services;
    }
}

//using Microsoft.OpenApi.Models;

//namespace Bookory.API.Extensions;

//public static class AddSwaggerGenExtension
//{
//    public static IServiceCollection AddSwaggerGenService(this IServiceCollection services)
//    {
//        services.AddSwaggerGen(option =>
//        {
//            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
//            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//            {
//                In = ParameterLocation.Header,
//                Description = "Please enter a valid token",
//                Name = "Authorization",
//                Type = SecuritySchemeType.ApiKey,
//                BearerFormat = "JWT",
//                Scheme = "Bearer"
//            });
//            option.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {
//                    new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Type=ReferenceType.SecurityScheme,
//                            Id="Bearer"
//                        }
//                    },
//                    new string[]{}
//                }
//            });
//        });

//        return services;
//    }
//}

//using Microsoft.OpenApi.Models;

//namespace Bookory.API.Extensions;

//public static class AddSwaggerGenExtension
//{
//    public static IServiceCollection AddSwaggerGenService(this IServiceCollection services)
//    {
//        services.AddSwaggerGen(option =>
//        {
//            // Set up the basic API documentation with better descriptions.
//            option.SwaggerDoc("v1", new OpenApiInfo
//            {
//                Title = "Bookory API",
//                Version = "v1",
//                Description = "API documentation for the Bookory application. Use this API for book catalog management, authentication, and more."
//            });

//            // Add the Bearer authentication header.
//            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//            {
//                In = ParameterLocation.Header,
//                Description = "Please enter a valid JWT token",
//                Name = "Authorization",
//                Type = SecuritySchemeType.ApiKey,
//                BearerFormat = "JWT",
//                Scheme = "Bearer"
//            });

//            // Add the security requirement for Bearer authentication.
//            option.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {
//                    new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                        }
//                    },
//                    new string[] { }
//                }
//            });

//            // Optionally add a server URL for better clarity if needed in production.
//            option.AddServer(new OpenApiServer { Url = "http://shahin20sh-001-site1.qtempurl.com" });
//        });

//        return services;
//    }
//}

