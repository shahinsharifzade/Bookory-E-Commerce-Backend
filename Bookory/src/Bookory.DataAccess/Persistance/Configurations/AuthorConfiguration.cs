using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.Name).IsRequired(true).HasMaxLength(300);
        //builder.Property(p => p.Images).IsRequired(true);
        builder.Property(a => a.Biography).IsRequired(true).HasMaxLength(1000);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasMany(a => a.Images).WithOne(a => a.Author);
    }
}
