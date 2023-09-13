using Bookory.Business.Utilities.Security.Encrypting;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bookory.API.Extensions;

public static class AddJwtAuthenticationExtension
{
    public static IServiceCollection AddJwtAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        TokenOption tokenOption = configuration.GetSection("TokenOptions").Get<TokenOption>();
        string audience = tokenOption.Audience;
        string issuer = tokenOption.Issuer;
        string securityKey = tokenOption.SecurityKey;
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
