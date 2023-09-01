using Bookory.Business.Utilities.Security.Encrypting;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bookory.API.Extensions;

public static class AddJwtAuthenticationExtension
{
    public static IServiceCollection AddJwtAuthenticationService(this IServiceCollection services , string audience , string issuer, string securityKey)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(securityKey),
                LifetimeValidator = (_, expires, _, _) => expires != null ? expires > DateTime.UtcNow : false
            };
        });

        return services;
    }
}
