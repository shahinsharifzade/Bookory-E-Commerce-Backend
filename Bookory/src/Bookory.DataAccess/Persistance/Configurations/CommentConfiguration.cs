using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(c => c.User).WithMany( u => u.Comments);
        builder.HasMany(c => c.Replies).WithOne(r => r.RefComment);


        builder.Property(c => c.Content)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(c => c.EntityType)
            .IsRequired()
            .HasMaxLength(255);
    }
}
