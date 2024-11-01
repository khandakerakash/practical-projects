﻿using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Enums;
using MedicineShopApplication.BLL.Utils;

namespace MedicineShopApplication.BLL.Validations
{
    public class AdminUserRegistrationRequestDtoValidator : AbstractValidator<AdminUserRegistrationRequestDto>
    {
        public AdminUserRegistrationRequestDtoValidator()
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
                .WithMessage("User role must be one of the following: developer, superadmin, admin, manager, moderator, salesman etc.");

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                .NotEmpty()
                .Must(VaidateDateOfBirth)
                .WithMessage("The Admin user must be a minimum of 18 years old.");
        }

        private bool BeAvalidRole(string roleName)
        {
            var validRoles = new List<string>
            {
                UserRoleUtils.GetUserRole(UserRole.developer),
                UserRoleUtils.GetUserRole(UserRole.superadmin),
                UserRoleUtils.GetUserRole(UserRole.admin),
                UserRoleUtils.GetUserRole(UserRole.manager),
                UserRoleUtils.GetUserRole(UserRole.moderator),
                UserRoleUtils.GetUserRole(UserRole.salesman),
                UserRoleUtils.GetUserRole(UserRole.customer)
            };

            return validRoles.Contains(roleName);
        }

        private bool VaidateDateOfBirth(DateTime dateTime)
        {
            if (dateTime > DateTime.Now.AddYears(-18))
            {
                return false;
            }

            return true;
        }
    }
}
