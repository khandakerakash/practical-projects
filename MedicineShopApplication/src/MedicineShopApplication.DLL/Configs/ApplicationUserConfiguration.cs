using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.DLL.Models.General;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineShopApplication.DLL.Configs
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Configure User - Order (one-to-many) relasionship
            builder
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User - Payment (one-to-many) relasionship
            builder
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User - Cart (one-to-one) relasionship
            builder
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User - UserStatusChangeLog (one-to-many) relationship
            builder
                .HasMany(u => u.UserStatusChangeLogs)
                .WithOne(log => log.User)
                .HasForeignKey(log => log.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
