using Bookory.Core.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.DataAccessLayer.Persistance.Interceptors;

public class BaseEntityInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BaseEntityInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntity(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntity(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    void UpdateEntity(DbContext context)
    {
        if (context == null) return;
        var entries = context.ChangeTracker.Entries<BaseEntity>();

        foreach (var item in entries)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    item.Entity.CreatedAt = DateTime.UtcNow;
                    item.Entity.ModifiedAt = DateTime.UtcNow;
                    item.Entity.CreateBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                    break;

                case EntityState.Modified:
                    item.Entity.ModifiedAt = DateTime.UtcNow;
                    item.Entity.ModifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                    break;
            }
        }
    }
}
