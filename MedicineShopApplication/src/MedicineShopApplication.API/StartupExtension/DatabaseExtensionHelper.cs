using MedicineShopApplication.DLL.BogusData;
using MedicineShopApplication.DLL.DbContextInit;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.API.StartupExtension
{
    public static class DatabaseExtensionHelper
    {
        public static IServiceCollection AddDatabaseExtensionHelper(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IApplicationBuilder RunMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();

                // Seed data if the User, Category, Product and Inventory Tables are empty
                //if(!db.Users.Any() && !db.Categories.Any() && !db.Products.Any() && !db.Inventories.Any())
                //{
                //    var categories = DataSeeder.GenerateCategories(10);
                //    var products = DataSeeder.GenerateProducts(50, categories);
                //    var inventories = DataSeeder.GenerateInventories(50, products);
                //    var users = DataSeeder.GenerateUsers(20);

                //    db.Categories.AddRange(categories);
                //    db.Products.AddRange(products);
                //    db.Inventories.AddRange(inventories);
                //    db.Users.AddRange(users);
                //    db.SaveChanges();
                //}
            }
            return app;
        }
    }
}
