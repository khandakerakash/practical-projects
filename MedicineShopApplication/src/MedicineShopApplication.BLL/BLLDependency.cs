using MedicineShopApplication.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineShopApplication.BLL
{
    public static class BLLDependency
    {
        public static IServiceCollection AddBLLDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
