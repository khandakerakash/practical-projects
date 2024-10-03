using FluentValidation;
using MedicineShopApplication.BLL.Dtos.UnitOfMeasure;

namespace MedicineShopApplication.BLL.Validations
{
    public class CreateUnitOfMeasureRequestDtoValidator : AbstractValidator<CreateUnitOfMeasureRequestDto>
    {
        public CreateUnitOfMeasureRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(24);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(255);
        }
    }
}
