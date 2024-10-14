using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Brand;

namespace MedicineShopApplication.BLL.Validations
{
    public class UpdateBrandRequestDtoValidator : AbstractValidator<UpdateBrandRequestDto>
    {
        public UpdateBrandRequestDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(10);

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty();
        }
    }
}
