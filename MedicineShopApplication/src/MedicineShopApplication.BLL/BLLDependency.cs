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
using MedicineShopApplication.BLL.Dtos.Cart;
using MedicineShopApplication.BLL.Dtos.Inventory;
using MedicineShopApplication.BLL.Dtos.Order;

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
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IInventoryService, InventoryService>();


            // Validation dependency
            AddValidatorDependencies(services);

            return services;
        }

        private static void AddValidatorDependencies(IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUserRequestDtoValidator>();
            services.AddScoped<IValidator<AdminUserRegistrationRequestDto>, AdminUserRegistrationRequestDtoValidator>();
            services.AddScoped<IValidator<AdminUserUpdateRequestDto>, UpdateAdminUserRequestDtoValidator>();
            services.AddScoped<IValidator<CustomerUserRegistrationRequestDto>, CustomerUserRegistrationRequestDtoValidator>();
            services.AddScoped<IValidator<CustomerUserUpdateRequestDto>, CustomerUserUpdateRequestDtoValidator>();
            services.AddScoped<IValidator<CreateCategoryRequestDto>, CreateCategoryRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateCategoryRequestDto>, UpdateCategoryRequestDtoValidator>();
            services.AddScoped<IValidator<CreateProductRequestDto>, CreateProductRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateProductRequestDto>, UpdateProductRequestDtoValidator>();
            services.AddScoped<IValidator<CreateInventoryRequestDto>, CreateInventoryRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateInventoryRequestDto>, UpdateInventoryRequestDtoValidator>();
            services.AddScoped<IValidator<CreateBrandRequestDto>, CreateBrandRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateBrandRequestDto>, UpdateBrandRequestDtoValidator>();
            services.AddScoped<IValidator<CreateUnitOfMeasureRequestDto>, CreateUnitOfMeasureRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateUnitOfMeasureRequestDto>, UpdateUnitOfMeasureRequestDtoValidator>();
            services.AddScoped<IValidator<AddToCartRequestDto>, AddToCartRequestDtoValidator>();
            services.AddScoped<IValidator<UpdateToCartRequestDto>, UpdateToCartRequestDtoValidator>();
            services.AddScoped<IValidator<CreateOrderRequestDto>, CreateOrderRequestDtoValidator>();
        }
    }
}
