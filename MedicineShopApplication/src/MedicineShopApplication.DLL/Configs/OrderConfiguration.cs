using MedicineShopApplication.DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Order - OrderItem (one-to-many) relationship
            builder
                .HasMany(oi => oi.OrderItems)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order - Payment (one-to-one) relationship
            builder
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order - Invoice (one-to-one) relationship
            builder
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
