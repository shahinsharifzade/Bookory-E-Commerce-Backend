//using Bookory.Core.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Bookory.DataAccess.Persistance.Configurations;

//public class OrderConfiguration : IEntityTypeConfiguration<Order>
//{
//    public void Configure(EntityTypeBuilder<Order> builder)
//    {
//        builder.HasOne(o => o.ShoppingSession)
//                 .WithOne(s => s.Order)
//                 .HasForeignKey<Order>(o => o.ShoppingSessionId);
//    }
//}
