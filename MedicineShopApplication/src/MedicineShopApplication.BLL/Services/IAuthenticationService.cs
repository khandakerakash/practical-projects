
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Authentication;
using MedicineShopApplication.BLL.Enums;

namespace MedicineShopApplication.BLL.Services
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<string>> Register(RegisterUserRequestDto request);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
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
                PhoneNumberConfirmed = true
            };

            var userCreationResponse = await _userManager.CreateAsync(user);

            if(!userCreationResponse.Succeeded)
            {
                return new ApiResponse<string>(userCreationResponse.Errors);
            }

            var roleAssignmentResponse = await _userManager.AddToRoleAsync(user, UserRole.customer.ToString());

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
