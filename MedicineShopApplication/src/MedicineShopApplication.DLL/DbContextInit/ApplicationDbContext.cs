using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.Configs;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MedicineShopApplication.DLL.DbContextInit
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<UserStatusChangeLog> UserStatusChangeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SoftDeleteFunctionality(modelBuilder);
            EntityConfigures(modelBuilder);
        }

        // Apply global filter for soft delete
        private void SoftDeleteFunctionality(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(bool));
                    var isDeletedProperty =
                        Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
                    var compareExpression =
                        Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                    var lambda = Expression.Lambda(compareExpression, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        private void EntityConfigures(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        }

        public async Task<int> SaveChangesAsync()
        {
            //await OnBeforeSaveChanges();

            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
            }


            return await base.SaveChangesAsync();
        }
    }
}
