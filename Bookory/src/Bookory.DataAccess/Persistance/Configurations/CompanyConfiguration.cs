using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(c => c.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(c => c.Description).IsRequired(true).HasMaxLength(200);
        builder.Property(c => c.Logo).IsRequired(true);
        builder.Property(c => c.BannerImage).IsRequired(true);
        builder.Property(c => c.ContactEmail).IsRequired(true).HasMaxLength(50);
        builder.Property(c => c.ContactPhone).IsRequired(true).HasMaxLength(50);

        builder.HasMany(c => c.Books).WithOne( b => b.Company).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.User).WithOne(b => b.Company).HasForeignKey<Company>(c => c.UserId).OnDelete(DeleteBehavior.NoAction); 

        builder.Property(c => c.Rating).HasDefaultValue(0);
    }
}
