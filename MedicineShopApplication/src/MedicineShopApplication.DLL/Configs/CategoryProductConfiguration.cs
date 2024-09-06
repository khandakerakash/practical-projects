using MedicineShopApplication.DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            // Category - Product (many-to-many) relationship
            builder
                .HasKey(cp => new { cp.ProductId, cp.CategoryId });

            builder
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CategoryProducts)
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(cp => cp.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
