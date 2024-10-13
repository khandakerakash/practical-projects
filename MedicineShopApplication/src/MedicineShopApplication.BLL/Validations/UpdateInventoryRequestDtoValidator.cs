using FluentValidation;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.Models.Enums;
using MedicineShopApplication.BLL.Dtos.Inventory;

namespace MedicineShopApplication.BLL.Validations
{
    public class UpdateInventoryRequestDtoValidator : AbstractValidator<UpdateInventoryRequestDto>
    {
        public UpdateInventoryRequestDtoValidator()
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

            RuleFor(x => x.Status)
                .NotNull()
                .NotEmpty()
                .Must(BeAvalidStatus).WithMessage("Invalid inventory status.")
                .WithMessage("Inventory status must be one of the following: Available, OutOfStock, Reordered etc.");
        }

        private bool BeAvalidStatus(string status)
        {
            var validstatus = new List<string>
            {
                InventoryStatusUtils.GetInventoryStatus(InventoryStatus.Available),
                InventoryStatusUtils.GetInventoryStatus(InventoryStatus.OutOfStock),
                InventoryStatusUtils.GetInventoryStatus(InventoryStatus.Reordered),
            };

            return validstatus.Contains(status);
        }
    }
}
