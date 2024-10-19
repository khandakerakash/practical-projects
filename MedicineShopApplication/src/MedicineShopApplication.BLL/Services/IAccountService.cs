
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Enums;
using MedicineShopApplication.BLL.Dtos.Account;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Services
{
    public interface IAccountService
    {
        Task<ApiResponse<string>> Register(RegisterUserRequestDto request);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResponse<string>> Register(RegisterUserRequestDto request)
        {
            var validator = new RegisterUserRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            if(await UserExistsAsync(request))
            {
                return new ApiResponse<string>(null, false, "User with this phone number already exists.");
            }

            var user = new ApplicationUser()
            {
                PhoneNumber = request.PhoneNumber,
                UserName = request.PhoneNumber,
                PhoneNumberConfirmed = true,
                Status = UserStatus.Active
            };

            var userCreationResponse = await _userManager.CreateAsync(user, request.Password);

            if(!userCreationResponse.Succeeded)
            {
                return new ApiResponse<string>(userCreationResponse.Errors);
            }

            var roleAssignmentResponse = await _userManager.AddToRoleAsync(user, UserRole.developer.ToString());

            if(!roleAssignmentResponse.Succeeded)
            {
                return new ApiResponse<string>(roleAssignmentResponse.Errors);
            }

            return new ApiResponse<string>(null, true, "Welcome to our system.");
        }

        #region Helper methods for Register User
        /// <summary>
        /// Checks if a user with the given phone number already exists.
        /// </summary>
        /// <param name="request">The registration request containing the user's phone number.</param>
        /// <returns>Returns true if the user already exists, otherwise false.</returns>
        private async Task<bool> UserExistsAsync(RegisterUserRequestDto request)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            return existingUser != null;
        }

        #endregion
    }
}
