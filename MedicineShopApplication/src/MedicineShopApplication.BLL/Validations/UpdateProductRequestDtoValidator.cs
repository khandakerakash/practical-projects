using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Validations
{
    public class UpdateProductRequestDtoValidator : AbstractValidator<UpdateProductRequestDto>
    {
        public UpdateProductRequestDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(255);

            RuleFor(x => x.GenericName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(24);

            RuleFor(x => x.CostPrice)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.")
                .Must(HaveValidDecimalPlaces)
                .WithMessage("Price must not have more than two decimal places.");

            RuleFor(x => x.SellingPrice)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.")
                .Must(HaveValidDecimalPlaces)
                .WithMessage("Price must not have more than two decimal places.");

            RuleFor(x => x.Status)
                .IsEnumName(typeof(ProductStatus), caseSensitive: true)
                .WithMessage("Invalid product status value. Allowed values are: Available, OutOfStock, Discontinued.");
        }

        private bool HaveValidDecimalPlaces(decimal price)
        {
            return decimal.Round(price, 2) == price;
        }
    }
}
