using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Enums;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Customer;
using MedicineShopApplication.BLL.Dtos.Admin;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICustomerService
    {
        Task<ApiResponse<List<CustomerUserResponseDto>>> GetAllCustomers(PaginationRequest request);
        Task<ApiResponse<CustomerUserResponseDto>> GetCustomerById(int userId);
        Task<ApiResponse<CustomerUserResponseDto>> CreateCustomerUser(CustomerUserRegistrationRequestDto request, string userRoleName, int requestMaker);
        Task<ApiResponse<string>> UpdateCustomer(CustomerUserUpdateRequestDto request, int requestMaker);
        Task<ApiResponse<string>> DeleteCustomer(int userId, int requestMaker);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ApiResponse<List<CustomerUserResponseDto>>> GetAllCustomers(PaginationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<CustomerUserResponseDto>> GetCustomerById(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<CustomerUserResponseDto>> CreateCustomerUser(CustomerUserRegistrationRequestDto request, string userRoleName, int requestMaker)
        {
            var allowedRoles = new[]
            {
                UserRoleUtils.GetUserRole(UserRole.developer),
                UserRoleUtils.GetUserRole(UserRole.superadmin),
                UserRoleUtils.GetUserRole(UserRole.admin),
                UserRoleUtils.GetUserRole(UserRole.manager)
            };

            if (!allowedRoles.Contains(userRoleName))
            {
                return new ApiResponse<CustomerUserResponseDto>(null, false, "Only Developer, Super Admin, Admin or Manager can create customer users.");
            }

            var validator = new CustomerUserRegistrationRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CustomerUserResponseDto>(validationResult.Errors);
            }

            var isPhoneNumberOrUserExisting = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.PhoneNumber == request.PhoneNumber)
                .FirstOrDefaultAsync();

            if (isPhoneNumberOrUserExisting.HasValue())
            {
                return new ApiResponse<CustomerUserResponseDto>(null, false, "Customer with this Phone number already exists in our system.");
            }

            var isEmailExiting = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.Email == request.Email)
                .FirstOrDefaultAsync();

            if (isEmailExiting.HasValue())
            {
                return new ApiResponse<CustomerUserResponseDto>(null, false, "Customer with this email already exists in our system.");
            }

            var user = new ApplicationUser()
            {
                PhoneNumber = request.PhoneNumber,
                UserName = request.PhoneNumber,
                PhoneNumberConfirmed = true,
                Email = request.Email,
                Title = request.Title,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedBy = requestMaker
            };

            var userCreationResponse = await _userManager.CreateAsync(user, request.Password);

            if (!userCreationResponse.Succeeded)
            {
                return new ApiResponse<CustomerUserResponseDto>(userCreationResponse.Errors);
            }

            var userRole = Enum.Parse<UserRole>(request.UserRoleName, true);

            var roleAssignmentResponse = await _userManager.AddToRoleAsync(user, userRole.ToString());

            if (!roleAssignmentResponse.Succeeded)
            {
                return new ApiResponse<CustomerUserResponseDto>(roleAssignmentResponse.Errors);
            }

            return new ApiResponse<CustomerUserResponseDto>(null, true, "The Customer user has been created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateCustomer(CustomerUserUpdateRequestDto request, int requestMaker)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> DeleteCustomer(int userId, int requestMaker)
        {
            throw new NotImplementedException();
        }

        #region Helper methods for customer user

        #endregion
    }
}
