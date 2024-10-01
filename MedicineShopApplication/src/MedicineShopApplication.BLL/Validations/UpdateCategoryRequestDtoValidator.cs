using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Category;

namespace MedicineShopApplication.BLL.Validations
{
    public class UpdateCategoryRequestDtoValidator : AbstractValidator<UpdateCategoryRequestDto>
    {
        public UpdateCategoryRequestDtoValidator() 
        {
            RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(15)
                .MaximumLength(320);
        }
    }
}
