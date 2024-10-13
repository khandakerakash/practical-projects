using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Inventory;

namespace MedicineShopApplication.BLL.Validations
{
    public class CreateInventoryRequestDtoValidator : AbstractValidator<CreateInventoryRequestDto>
    {
        public CreateInventoryRequestDtoValidator() 
        { 
            RuleFor(x => x.ProductId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than zero (0).");

            RuleFor(x => x.QuantityInStock)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Quantity in stock must be greater than zero (0).");

            RuleFor(x => x.Location)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Notes)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UnitOfMeasureId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("UnitOfMeasureId must be greater than zero (0).");
        }
    }
}
