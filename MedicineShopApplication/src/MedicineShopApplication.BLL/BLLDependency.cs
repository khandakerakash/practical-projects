﻿

using Microsoft.Extensions.DependencyInjection;

namespace MedicineShopApplication.BLL
{
    public static class BLLDependency
    {
        public static IServiceCollection AddBLLDependency(this IServiceCollection services)
        {
            return services;
        }
    }
}