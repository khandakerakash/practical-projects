using MedicineShopApplication.DLL.BogusData;
using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.General;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.API.StartupExtension
{
    public static class DatabaseExtensionHelper
    {
        public static IServiceCollection AddDatabaseExtensionHelper(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                option.UseOpenIddict<int>();
            });

            return services;
        }

        public static IApplicationBuilder RunMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();

            var categories = new List<Category>();
            var brands = new List<Brand>();
            var unitOfMeasures = new List<UnitOfMeasure>();
            var products = new List<Product>();
                
            // Application user role seed
            DataSeeder.GenerateRolesWithUser(scope.ServiceProvider);

            if (!db.Categories.Any())
            {
                categories = DataSeeder.GenerateCategories(10);
                db.Categories.AddRange(categories);
            }

            if (!db.Brands.Any())
            {
                brands = DataSeeder.GenerateBrands(10);
                db.Brands.AddRange(brands);
            }
                
            if (!db.UnitOfMeasures.Any())
            {
                unitOfMeasures = DataSeeder.GenerateUnitsOfMeasure(10);
                db.UnitOfMeasures.AddRange(unitOfMeasures);
            }
                
            if (!db.Products.Any())
            {
                products = DataSeeder.GenerateProducts(50, categories,brands,unitOfMeasures);
                db.Products.AddRange(products);
            }
                
            if (!db.Inventories.Any())
            {
                var  inventories = DataSeeder.GenerateInventories(50, products);
                db.Inventories.AddRange(inventories);
            }
                

            // Application seed
            DataSeeder.GenerateApplication(scope.ServiceProvider);
            return app;
        }
    }
}
