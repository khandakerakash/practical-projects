using MedicineShopApplication.DLL.UOW;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineShopApplication.DLL
{
    public static class DLLDependency
    {
        public static IServiceCollection AddDLLDependency(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
