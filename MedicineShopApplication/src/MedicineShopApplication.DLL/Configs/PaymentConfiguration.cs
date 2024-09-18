using MedicineShopApplication.DLL.Models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Setting precision and scale for decimal properties
            builder
                .Property(p => p.PaymentAmount)
                .HasPrecision(18, 2);
        }
    }
}
