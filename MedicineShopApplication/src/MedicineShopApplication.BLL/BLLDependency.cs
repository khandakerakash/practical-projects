using FluentValidation;
using MedicineShopApplication.BLL.Services;
using MedicineShopApplication.BLL.Validations;
using Microsoft.Extensions.DependencyInjection;
using MedicineShopApplication.BLL.Dtos.Authentication;
using MedicineShopApplication.BLL.Dtos.Category;

namespace MedicineShopApplication.BLL
{
    public static class BLLDependency
    {
        public static IServiceCollection AddBLLDependency(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();


            // Validation dependency
            AddValidatorDependencies(services);

            return services;
        }

        private static void AddValidatorDependencies(IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUserRequestDtoValidator>();
            services.AddScoped<IValidator<CreateCategoryRequestDto>, CreateCategoryRequestDtoValidator>();
        }
    }
}
