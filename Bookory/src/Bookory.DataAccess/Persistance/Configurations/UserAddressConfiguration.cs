using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.Property(ua => ua.AddressLine1).IsRequired(true).HasMaxLength(300);
        builder.Property(ua => ua.City).IsRequired(true).HasMaxLength(300);
        builder.Property(ua => ua.Country).IsRequired(true).HasMaxLength(300);
        builder.Property(ua => ua.Mobile).IsRequired(true).HasMaxLength(300);
        builder.Property(ua => ua.PostalCode).IsRequired(true).HasMaxLength(300);

        builder.HasOne(ua => ua.User).WithMany(u => u.UserAddresses);
    }
}
