using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MedicineShopApplication.DLL.Configs;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.DLL.Models.Common;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;


namespace MedicineShopApplication.DLL.DbContextInit
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        private readonly DbContextOptions options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions options, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) 
            : base(options)
        {
            this.options = options;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

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
        public DbSet<AuditLog> AuditLogs { get; set; }


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
            await OnBeforeSaveChanges();

            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync();
        }

        private async Task OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var request = _httpContextAccessor?.HttpContext?.Request;
            var auditEntries = new List<AuditEntry>();
            var userId = _httpContextAccessor?.HttpContext?.User.GetUserName() ?? "System";
            var controller = request?.HttpContext?.GetRouteData()?.Values["controller"]?.ToString();
            var action = request?.HttpContext?.GetRouteData()?.Values["action"]?.ToString();

            // Load excluded properties from configuration
            var excludedProperties = _configuration
                .GetSection("AuditSettings:ExcludedProperties")
                .Get<List<string>>() ?? new List<string>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    Action = entry.State.ToString(),
                    Timestamp = DateTime.UtcNow,
                    UserName = userId,
                    ActionName = action,
                    ControllerName = controller,
                };

                foreach (var property in entry.Properties)
                {
                    // Check if the property should be excluded based on the ExcludeFromAuditAttribute
                    //var propertyInfo = entry.Entity.GetType().GetProperty(property.Metadata.Name);
                    // if (propertyInfo != null && Attribute.IsDefined(propertyInfo, typeof(ExcludeFromAuditAttribute)))
                    // {
                    //     // Skip adding this property to the audit log if it's marked with ExcludeFromAuditAttribute
                    //     continue;
                    // }

                    // Skip excluded properties
                    var propertyInfo = property.Metadata.Name;
                    if (excludedProperties.Contains(propertyInfo))
                    {
                        continue;
                    }

                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified &&
                                property.OriginalValue?.ToString() != property.CurrentValue?.ToString())
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }

                auditEntries.Add(auditEntry);
            }

            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAuditLog());
            }
        }
    }
}
