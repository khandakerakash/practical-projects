using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Category;

namespace MedicineShopApplication.BLL.Validations
{
    public class CreateCategoryRequestDtoValidator : AbstractValidator<CreateCategoryRequestDto>
    {
        public CreateCategoryRequestDtoValidator()
        {
            //RuleFor(x => x.Code)
            //   .NotNull()
            //   .NotEmpty()
            //   .MinimumLength(8)
            //   .MaximumLength(8)
            //   .Matches(@"^[A-Z]{3}-\d{4}$")
            //   .WithMessage("Invalid code format. Code must be in the format of 'ABC-1234'.");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(24)
                .MaximumLength(320);
        }
    }
}
