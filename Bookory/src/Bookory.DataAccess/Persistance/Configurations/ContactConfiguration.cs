using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Email).IsRequired();
        builder.Property(c => c.Message).IsRequired().HasMaxLength(300);
    }
}
