using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
{
    public void Configure(EntityTypeBuilder<BookGenre> builder)
    {
        builder
        .HasKey(bc => new { bc.BookId, bc.GenreId });


        builder.HasOne(bg => bg.Book)
        .WithMany(b => b.BookGenres)
        .HasForeignKey(bc => bc.BookId); ;

        builder.HasOne(bg => bg.Genre)
        .WithMany(c => c.BookGenres)
        .HasForeignKey(bc => bc.GenreId); ;

        builder.Property(b => b.BookId).IsRequired(true);
        builder.Property(b => b.GenreId).IsRequired(true);
    }
}
