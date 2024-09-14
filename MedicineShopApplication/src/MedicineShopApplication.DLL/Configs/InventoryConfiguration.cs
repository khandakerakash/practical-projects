using MedicineShopApplication.DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            // Setting precision and scale for decimal properties
            builder
                .Property(i => i.CostPrice)
                .HasPrecision(18, 2);

            builder
                .Property(i => i.SellingPrice)
                .HasPrecision(18, 2);
        }
    }
}
