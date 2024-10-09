using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Admin;

namespace MedicineShopApplication.BLL.Validations
{
    public class UpdateAdminUserRequestDtoValidator : AbstractValidator<AdminUserUpdateRequestDto>
    {
        public UpdateAdminUserRequestDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(3);

            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(255);
        }
    }
}
