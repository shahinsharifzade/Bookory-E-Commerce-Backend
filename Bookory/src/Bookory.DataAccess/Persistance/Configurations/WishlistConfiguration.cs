using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.HasOne(w => w.User).WithOne(u => u.Wishlist);
        builder.HasMany(w => w.Books).WithMany(b => b.Wishlists).UsingEntity(j => j.ToTable("BookWishlist"));
    }
}
