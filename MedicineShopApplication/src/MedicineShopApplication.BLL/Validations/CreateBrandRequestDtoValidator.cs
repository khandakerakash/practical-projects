using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Brand;

namespace MedicineShopApplication.BLL.Validations
{
    public class CreateBrandRequestDtoValidator : AbstractValidator<CreateBrandRequestDto>
    {
        public CreateBrandRequestDtoValidator() 
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
        }
    }
}
