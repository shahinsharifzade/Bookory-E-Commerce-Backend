using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.HasOne(ci => ci.ShoppingSession)
            .WithMany(ci => ci.BasketItems)
            .HasForeignKey(ci => ci.SessionId);

        builder.HasOne(ci => ci.Book)
            .WithMany(b => b.BasketItems)
            .HasForeignKey(ci => ci.BookId)
            .HasConstraintName("FK_BasketItem_To_Book_BookKey"); ;
    }
}
