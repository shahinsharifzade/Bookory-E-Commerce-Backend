using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

internal class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.Property(b => b.Title).IsRequired(true).HasMaxLength(200);
        builder.Property(b => b.Image).IsRequired(true);
        builder.Property(b => b.Content).IsRequired(true).HasMaxLength(2000);

        builder.HasMany(b => b.Categories).WithMany(c => c.Blogs).UsingEntity(j => j.ToTable("BlogCategories"));
    }
}
