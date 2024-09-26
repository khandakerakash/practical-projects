using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.Models.General;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Setting precision and scale for decimal properties
            builder
                .Property(p => p.CostPrice)
                .HasPrecision(18, 2);

            builder
                .Property(p => p.SellingPrice)
                .HasPrecision(18, 2);

            // Product - CartItem (one-to-many) relationship
            builder
                .HasMany(p => p.CartItems)
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - OrderItem (one-to-many) relationship
            builder
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - Inventory (one-to-many) relationship
            builder
                .HasMany(i => i.Inventories)
                .WithOne(p => p.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
