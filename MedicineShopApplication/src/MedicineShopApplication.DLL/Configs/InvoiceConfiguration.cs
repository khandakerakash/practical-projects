using MedicineShopApplication.DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // Setting precision and scale for decimal properties
            builder
                .Property(i => i.TotalAmount)
                .HasPrecision(18, 2);
        }
    }
}
