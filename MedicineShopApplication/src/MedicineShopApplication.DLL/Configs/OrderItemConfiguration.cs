using MedicineShopApplication.DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Setting precision and scale for decimal properties
            builder
                .Property(x => x.Price)
                .HasPrecision(18, 2);
        }
    }
}
