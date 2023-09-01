using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class AuthorImageConfiguration : IEntityTypeConfiguration<AuthorImage>
{
    public void Configure(EntityTypeBuilder<AuthorImage> builder)
    {
        builder.HasOne(ai => ai.Author).WithMany(a => a.Images).HasForeignKey(ai => ai.AuthorId);
    }
}
