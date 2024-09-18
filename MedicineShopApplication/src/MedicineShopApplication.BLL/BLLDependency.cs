﻿using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Authentication;
using MedicineShopApplication.BLL.Services;
using MedicineShopApplication.BLL.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineShopApplication.BLL
{
    public static class BLLDependency
    {
        public static IServiceCollection AddBLLDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();


            // Validation dependency
            AddValidatorDependencies(services);

            return services;
        }

        private static void AddValidatorDependencies(IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUserRequestValidatorDto>();
        }
    }
}
