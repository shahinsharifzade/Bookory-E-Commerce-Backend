using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class ShoppingSessionConfiguration : IEntityTypeConfiguration<ShoppingSession>
{
    public void Configure(EntityTypeBuilder<ShoppingSession> builder)
    {
        builder.HasOne(ss => ss.User).WithMany(u => u.ShoppingSessions).HasForeignKey(ss => ss.UserId);
    }
}
