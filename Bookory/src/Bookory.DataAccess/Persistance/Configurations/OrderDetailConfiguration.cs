using Bookory.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookory.DataAccess.Persistance.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasOne(od => od.User).WithMany( u => u.OrderDetails);
        builder.HasOne(od => od.PaymentDetail).WithOne();
        builder.HasMany(od => od.OrderItems).WithOne(oi => oi.OrderDetail);
    }
}
