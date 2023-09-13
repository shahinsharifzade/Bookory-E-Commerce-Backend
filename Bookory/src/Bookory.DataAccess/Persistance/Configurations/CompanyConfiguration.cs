using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasMany(c => c.Books).WithOne( b => b.Company).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.User).WithOne(b => b.Company).HasForeignKey<Company>(c => c.UserId).OnDelete(DeleteBehavior.NoAction); 

    }
}
