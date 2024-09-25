using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Account;

namespace MedicineShopApplication.BLL.Validations
{
    public class RegisterUserRequestDtoValidator : AbstractValidator<RegisterUserRequestDto>
    {
        public RegisterUserRequestDtoValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotNull().NotEmpty()
                .MinimumLength(11)
                .MinimumLength(11)
                .Matches(@"^01[3-9]\d{8}$")
                .WithMessage("Invalid Bangladeshi mobile number.");

            RuleFor(x => x.Password)
                .NotNull().NotEmpty()
                .MinimumLength(5)
                .MaximumLength(5)
                .Matches(@"^\d{5}$")
                .WithMessage("Password must be 5 character number only.");
        }
    }
}
