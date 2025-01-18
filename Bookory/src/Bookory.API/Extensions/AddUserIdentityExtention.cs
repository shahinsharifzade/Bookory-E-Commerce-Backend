using Bookory.Core.Models.Identity;
using Bookory.DataAccess.Initalizers;
using Bookory.DataAccess.Persistance.Context.EfCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Bookory.API.Extensions;

public static class AddUserIdentityExtention
{
    public static IServiceCollection AddUserIdentityService(this IServiceCollection service)
    {
        service.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        return service;
    }


    public static async Task InitDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<DbContextInitalizer>();
            await initializer.InitDatabaseAsync();
        }
    }
}
