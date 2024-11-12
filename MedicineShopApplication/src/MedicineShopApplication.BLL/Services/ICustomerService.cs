using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Enums;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.Enums;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Customer;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICustomerService
    {
        Task<ApiResponse<List<CustomerUserResponseDto>>> GetAllCustomers(PaginationRequest request);
        Task<ApiResponse<CustomerUserResponseDto>> GetCustomerById(int userId);
        Task<ApiResponse<CustomerUserResponseDto>> CreateCustomerUser(CustomerUserRegistrationRequestDto request, string userRoleName, int requestMaker);
        Task<ApiResponse<string>> UpdateCustomerUser(CustomerUserUpdateRequestDto request, int userId, int requestMaker);
        Task<ApiResponse<string>> DeleteCustomer(int userId, int requestMaker);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CustomerService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse<List<CustomerUserResponseDto>>> GetAllCustomers(PaginationRequest request)
        {
            var skipValue = PaginationUtils.SkipValue(request.Page, request.PageSize);

            var roleType = UserRoleUtils.GetRoleType(RoleType.customer).ToString();
            var customerUserRoleId = await _roleManager.Roles
                .Where(x => x.RoleType == roleType)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var customerUserIds = await _unitOfWork.RoleRepository
                .GetUserIdsByRoleIdAsync(customerUserRoleId);

            var usersQuery = _unitOfWork.UserRepository.FindAllAsync();

            usersQuery = usersQuery.Where(u => customerUserIds.Contains(u.Id));
            usersQuery = SortUsersQuery(usersQuery, request.SortBy);
            var totalCustomerUserCount = await usersQuery.CountAsync();

            var adminUserRoleIds = await _roleManager.Roles
                .Where(x => x.RoleType != roleType)
                .Select(x => x.Id)
                .ToListAsync();

            var adminUserIds = await _unitOfWork.RoleRepository
                .GetUserIdsByRoleIdsAsync(adminUserRoleIds);

            var adminUsersQuery = _unitOfWork.UserRepository.FindAllAsync();
            var adminUsers = await adminUsersQuery
                .Where(u => adminUserIds.Contains(u.Id))
                .ToListAsync();

            var paginatedUsers = await usersQuery
                .Skip(skipValue)
                .Take(request.PageSize)
                .ToListAsync();

            var userIds = adminUsers
                .Select(x => x.CreatedBy)
                .Union(usersQuery.Where(x => x.UpdatedBy.HasValue).Select(x => x.UpdatedBy.Value))
                .Distinct();

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var userRoles = await _unitOfWork.RoleRepository
                .GetRolesForUsersAsync(paginatedUsers.Select(u => u.Id));

            var rolesByUserId = userRoles
                .GroupBy(r => r.UserId)
                .ToDictionary(g => g.Key, g => g.Select(r => r.RoleName)
                .FirstOrDefault());

            var userListData = paginatedUsers
                .Select(user =>
                {
                    var userRoleName = rolesByUserId.ContainsKey(user.Id) ? rolesByUserId[user.Id] : "Unknown";

                    return new CustomerUserResponseDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        UserRoleName = userRoleName,
                        RoleTypeName = roleType,
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Status = UserStatusUtils.GetUserStatus(user.Status),
                        Gender = GenderUtils.GetGender(user.Gender),
                        DateOfBirth = user.DateOfBirth,
                        NationalIdentityCard = user.NationalIdentityCard,

                        CreatedAt = user.CreatedAt,
                        CreatedBy = user.CreatedBy,
                        CreatedByName = users.ContainsKey(user.CreatedBy)
                                        ? users[user.CreatedBy].GetFullName()
                                        : "",

                        UpdatedAt = user.UpdatedAt,
                        UpdatedBy = user.UpdatedBy,
                        UpdatedByName = user.UpdatedBy.HasValue && users.ContainsKey(user.UpdatedBy.Value)
                                        ? users[user.UpdatedBy.Value].GetFullName()
                                        : ""
                    };
                })
                .ToList();


            return new ApiPaginationResponse<List<CustomerUserResponseDto>>(userListData, request.Page, request.PageSize, totalCustomerUserCount);
        }

        public async Task<ApiResponse<CustomerUserResponseDto>> GetCustomerById(int userId)
        {
            var roleType = UserRoleUtils.GetRoleType(RoleType.customer).ToString();
            var customerUserRoleId = await _roleManager.Roles
                .Where(x => x.RoleType == roleType)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var customerUserIds = await _unitOfWork.RoleRepository
                .GetUserIdsByRoleIdAsync(customerUserRoleId);

            var userQuery = _unitOfWork.UserRepository
                .FindByConditionAsync(x => x.Id == userId);

            userQuery = userQuery.Where(u => customerUserIds.Contains(u.Id));

            var user = await userQuery.FirstOrDefaultAsync();
            if (user.HasNoValue())
            {
                return new ApiResponse<CustomerUserResponseDto>(null, false, "Customer user not found.");
            }

            var userRole = await _userManager
                .GetRolesAsync(user);

            var userResponse = await userQuery
                .Select(x => new CustomerUserResponseDto
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    UserRoleName = userRole.FirstOrDefault(),
                    RoleTypeName = roleType,
                    Title = x.Title,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Status = UserStatusUtils.GetUserStatus(x.Status),
                    Gender = GenderUtils.GetGender(x.Gender),
                    DateOfBirth = x.DateOfBirth,
                    NationalIdentityCard = x.NationalIdentityCard,
                    PostalCode = x.PostalCode,
                    PoliceStation = x.PoliceStation,
                    District = x.District,
                    Division = x.Division,
                    Address = x.Address
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<CustomerUserResponseDto>(userResponse, true, "Customer user found.");
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
                DateOfBirth = request.DateOfBirth,
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

        public async Task<ApiResponse<string>> UpdateCustomerUser(CustomerUserUpdateRequestDto request, int userId, int requestMaker)
        {
            if (userId != requestMaker)
            {
                return new ApiResponse<string>(null, false, "You do not have the necessary permissions to update customer information.");
            }

            var validator = new CustomerUserUpdateRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var users = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.Id == userId || x.Email == request.Email)
                .ToListAsync();

            var user = users.FirstOrDefault(x => x.Id == userId);
            if (user.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Customer user not found.");
            }

            var isEmailExiting = users.Any(x => x.Id != userId && x.Email == request.Email);
            if (isEmailExiting)
            {
                return new ApiResponse<string>(null, false, "Customer with this email already exists.");
            }

            if (!Enum.TryParse<UserStatus>(request.Status, true, out var status))
            {
                return new ApiResponse<string>(null, false, $"Requested status '{request.Status}' was not found.");
            }

            if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
            {
                return new ApiResponse<string>(null, false, $"Requested gender '{request.Gender}' was not found.");
            }


            user.Title = request.Title;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Status = status;
            user.Gender = gender;
            user.DateOfBirth = request.DateOfBirth;
            user.NationalIdentityCard = request.NationalIdentityCard;
            user.PostalCode = request.PostalCode;
            user.PoliceStation = request.PoliceStation;
            user.District = request.District;
            user.Division = request.Division;
            user.Address = request.Address;
            user.UpdatedBy = userId;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.UserRepository.Update(user);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the customer user.");
            }

            return new ApiResponse<string>(null, true, "The customer user updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteCustomer(int userId, int requestMaker)
        {
            throw new NotImplementedException();
        }

        #region Helper methods for customer user
        /// <summary>
        /// Sorts the provided <see cref="IQueryable{ApplicationUser}"/> based on the specified sorting criteria.
        /// The sorting criteria are provided as a string in the format "property direction", 
        /// where "direction" can be "asc" for ascending or "desc" for descending. 
        /// Multiple criteria can be specified, separated by commas, e.g., "name desc, code asc".
        /// If no valid criteria are specified, the default sorting is by BrandId in descending order.
        /// </summary>
        /// <param name="ApplicationUser">The queryable collection of <see cref="ApplicationUser"/> to be sorted.</param>
        /// <param name="sortBy">A string containing sorting criteria, e.g., "name desc, code asc".</param>
        /// <returns>An <see cref="IQueryable{ApplicationUser}"/> sorted based on the specified criteria.</returns>
        private IQueryable<ApplicationUser> SortUsersQuery(IQueryable<ApplicationUser> usersQuery, string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                // Default sorting
                return usersQuery.OrderByDescending(x => x.Id);
            }

            // Split multiple sort criteria (e.g., "name desc, code asc")
            var sortParts = sortBy.Split(',');

            foreach (var sortPart in sortParts)
            {
                var trimmedPart = sortPart.Trim();
                var orderDescending = trimmedPart.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
                var propertyName = orderDescending
                    ? trimmedPart[0..^5]
                    : trimmedPart.EndsWith(" asc", StringComparison.OrdinalIgnoreCase)
                        ? trimmedPart[0..^4]
                        : trimmedPart;


                if (propertyName.ToLower() == "id")
                {
                    usersQuery = orderDescending
                        ? usersQuery.OrderByDescending(x => x.Id)
                        : usersQuery.OrderBy(x => x.Id);
                }

                if (propertyName.ToLower() == "title")
                {
                    usersQuery = orderDescending
                        ? usersQuery.OrderByDescending(x => x.Title)
                        : usersQuery.OrderBy(x => x.Title);
                }

                if (propertyName.ToLower() == "firstname")
                {
                    usersQuery = orderDescending
                        ? usersQuery.OrderByDescending(x => x.FirstName)
                        : usersQuery.OrderBy(x => x.FirstName);
                }

                if (propertyName.ToLower() == "lastname")
                {
                    usersQuery = orderDescending
                        ? usersQuery.OrderByDescending(x => x.LastName)
                        : usersQuery.OrderBy(x => x.LastName);
                }

                if (propertyName.ToLower() == "email")
                {
                    usersQuery = orderDescending
                        ? usersQuery.OrderByDescending(x => x.Email)
                        : usersQuery.OrderBy(x => x.Email);
                }

                if (propertyName.ToLower() == "phonenumber")
                {
                    usersQuery = orderDescending
                        ? usersQuery.OrderByDescending(x => x.PhoneNumber)
                        : usersQuery.OrderBy(x => x.PhoneNumber);
                }

                if (propertyName.ToLower() == "address")
                {
                    usersQuery = orderDescending
                        ? usersQuery.OrderByDescending(x => x.Address)
                        : usersQuery.OrderBy(x => x.Address);
                }
            }

            return usersQuery; ;
        }
        #endregion
    }
}
