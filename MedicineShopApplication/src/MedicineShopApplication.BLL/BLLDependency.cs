using FluentValidation;
using MedicineShopApplication.BLL.Services;
using MedicineShopApplication.BLL.Dtos.Brand;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.BLL.Dtos.Account;
using Microsoft.Extensions.DependencyInjection;
using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.BLL.Dtos.UnitOfMeasure;
using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Dtos.Customer;

namespace MedicineShopApplication.BLL
{
    public static class BLLDependency
    {
        public static IServiceCollection AddBLLDependency(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IUnitOfMeasureService, UnitOfMeasureService>();


            // Validation dependency
            AddValidatorDependencies(services);

            return services;
        }

        private static void AddValidatorDependencies(IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUserRequestDtoValidator>();
            services.AddScoped<IValidator<AdminUserRegistrationRequestDto>, AdminUserRegistrationRequestDtoValidator>();
            services.AddScoped<IValidator<CustomerUserRegistrationRequestDto>, CustomerUserRegistrationRequestDtoValidator>();
            services.AddScoped<IValidator<CreateCategoryRequestDto>, CreateCategoryRequestDtoValidator>();
            services.AddScoped<IValidator<CreateProductRequestDto>, CreateProductRequestDtoValidator>();
            services.AddScoped<IValidator<CreateBrandRequestDto>, CreateBrandRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateBrandRequestDto>, UpdateBrandRequestDtoValidator>();
            services.AddScoped<IValidator<CreateUnitOfMeasureRequestDto>, CreateUnitOfMeasureRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateUnitOfMeasureRequestDto>, UpdateUnitOfMeasureRequestDtoValidator>();
        }
    }
}
