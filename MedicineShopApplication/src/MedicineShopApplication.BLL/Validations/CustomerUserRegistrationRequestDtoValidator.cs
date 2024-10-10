using FluentValidation;
using MedicineShopApplication.BLL.Enums;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.BLL.Dtos.Customer;

namespace MedicineShopApplication.BLL.Validations
{
    public class CustomerUserRegistrationRequestDtoValidator : AbstractValidator<CustomerUserRegistrationRequestDto>
    {
        public CustomerUserRegistrationRequestDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(5);

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
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotNull().NotEmpty()
                .MinimumLength(11)
                .MinimumLength(11)
                .Matches(@"^01[3-9]\d{8}$")
                .WithMessage("Invalid Bangladeshi mobile number.");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(5)
                .Matches(@"^\d{5}$")
                .WithMessage("Password must be 5 character number only.");

            RuleFor(x => x.UserRoleName)
                .NotNull()
                .NotEmpty()
                .Must(BeAvalidRole).WithMessage("Invalid user role.")
                .WithMessage("User role must be 'customer' to create a customer user.");
        }

        private bool BeAvalidRole(string roleName)
        {
            var validRoles = new List<string>
            {
                UserRoleUtils.GetUserRole(UserRole.customer)
            };

            return validRoles.Contains(roleName);
        }
    }
}
