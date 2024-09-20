using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Category;

namespace MedicineShopApplication.BLL.Validations
{
    public class CreateCategoryRequestDtoValidator : AbstractValidator<CreateCategoryRequestDto>
    {
        public CreateCategoryRequestDtoValidator()
        {
            RuleFor(x => x.Code)
               .NotNull().NotEmpty()
               .MinimumLength(2)
               .MinimumLength(20);

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MinimumLength(255);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(25)
                .MaximumLength(320);
        }
    }
}
