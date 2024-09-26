using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Product;

namespace MedicineShopApplication.BLL.Validations
{
    public class CreateProductRequestDtoValidator : AbstractValidator<CreateProductRequestDto>
    {
        public CreateProductRequestDtoValidator() 
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
        }

        private bool HaveValidDecimalPlaces(decimal price)
        {
            return decimal.Round(price, 2) == price;
        }
    }
}
