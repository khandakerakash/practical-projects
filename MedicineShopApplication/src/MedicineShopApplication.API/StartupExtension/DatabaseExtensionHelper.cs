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
            }
            return app;
        }
    }
}
