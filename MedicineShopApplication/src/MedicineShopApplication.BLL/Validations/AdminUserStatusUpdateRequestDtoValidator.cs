using FluentValidation;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Validations
{
    public class AdminUserStatusUpdateRequestDtoValidator : AbstractValidator<AdminUserStatusUpdateRequestDto>
    {
        public AdminUserStatusUpdateRequestDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Status)
                .NotNull()
                .NotEmpty()
                .Must(BeAvalidStatus).WithMessage("Invalid user status.")
                .WithMessage("User role must be one of the following: Active, Inactive, Pending, Banned, Suspended etc.");

            RuleFor(x => x.ReasonForChange)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(255);
        }

        private bool BeAvalidStatus(string statusName)
        {
            var statusNames = new List<string>
            {
                UserStatusUtils.GetUserStatus(UserStatus.Active),
                UserStatusUtils.GetUserStatus(UserStatus.Inactive),
                UserStatusUtils.GetUserStatus(UserStatus.Pending),
                UserStatusUtils.GetUserStatus(UserStatus.Banned),
                UserStatusUtils.GetUserStatus(UserStatus.Suspended)
            };

            return statusNames.Contains(statusName);
        }
    }
}
