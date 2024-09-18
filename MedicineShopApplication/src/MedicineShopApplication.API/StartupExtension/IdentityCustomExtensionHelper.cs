using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace MedicineShopApplication.API.StartupExtension
{
    public static class IdentityCustomExtensionHelper
    {
        public static IServiceCollection AddIdentityCustomExtensionHelper(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // username related modification
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = true;
            });

            // password related modification
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            return services;
        }

        public static IApplicationBuilder UseAppAuthentication(
            this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
