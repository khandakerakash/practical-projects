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

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.")
                .Must(HaveValidDecimalPlaces)
                .WithMessage("Price must not have more than two decimal places.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock must be zero or greater.");
        }

        private bool HaveValidDecimalPlaces(decimal price)
        {
            return decimal.Round(price, 2) == price;
        }
    }
}
