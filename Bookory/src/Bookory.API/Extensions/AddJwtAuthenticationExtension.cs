using Bookory.Business.Utilities.Security.Encrypting;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Bookory.API.Extensions;

public static class AddJwtAuthenticationExtension
{
    public static IServiceCollection AddJwtAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        TokenOption tokenOption = configuration.GetSection("TokenOptions").Get<TokenOption>();
        string audience = tokenOption.Audience;
        string issuer = tokenOption.Issuer;
        string securityKey = tokenOption.SecurityKey;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                //ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(securityKey),

                LifetimeValidator = (_, expires, _, _) => expires != null ? expires > DateTime.UtcNow : false,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },

                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.ContainsKey("jwt"))
                    {
                        context.Token = context.Request.Cookies["jwt"];
                    }
                    return Task.CompletedTask;
                },

                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    return context.Response.WriteAsJsonAsync(new
                    {
                        error = "Not authorized",
                        message = "You are not authorized to access this resource"
                    });
                }
            };
        });

        return services;
    }
}


//using Bookory.Business.Utilities.Security.Encrypting;
//using Bookory.Core.Models.Identity;
//using Microsoft.AspNetCore.Authentication.JwtBearer;

//namespace Bookory.API.Extensions;

//public static class AddJwtAuthenticationExtension
//{
//    public static IServiceCollection AddJwtAuthenticationService(this IServiceCollection services, IConfiguration configuration)
//    {
//        TokenOption tokenOption = configuration.GetSection("TokenOptions").Get<TokenOption>();
//        string audience = tokenOption.Audience;
//        string issuer = tokenOption.Issuer;
//        string securityKey = tokenOption.SecurityKey;
//        services.AddAuthentication(opt =>
//        {
//            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

//        }).AddJwtBearer(opt =>
//        {
//            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,

//                ValidIssuer = issuer,
//                ValidAudience = audience,
//                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(securityKey),
//                LifetimeValidator = (_, expires, _, _) => expires != null ? expires > DateTime.UtcNow : false
//            };
//        });

//        return services;
//    }
//}
