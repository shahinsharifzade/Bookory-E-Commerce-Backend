using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(p => p.AuthorId).IsRequired(true);

        builder.Property(p => p.Title).IsRequired(true).HasMaxLength(300);
        //builder.Property(p => p.Image).IsRequired(true);
        builder.Property(p => p.Description).IsRequired(true).HasMaxLength(300);
        builder.Property(p => p.StockQuantity).IsRequired(true);
        builder.Property(p => p.Price).IsRequired(true).HasColumnType("decimal(10,2)");
        builder.Property(p => p.DiscountPrice).IsRequired(false).HasColumnType("decimal(10,2)");
        builder.Property(p => p.SoldQuantity).HasDefaultValue(0);
        builder.Property(p => p.Rating).IsRequired(false);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasOne(a => a.Author).WithMany(b => b.Books);
    }
}
