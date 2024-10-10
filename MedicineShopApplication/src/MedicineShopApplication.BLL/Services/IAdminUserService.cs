using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Enums;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MedicineShopApplication.BLL.Services
{
    public interface IAdminUserService
    {
        Task<ApiResponse<List<AdminUserResponseDto>>> GetAllAdminUsers(PaginationRequest request);
        Task<ApiResponse<AdminUserResponseDto>> GetAdminUserById(int userId);
        Task<ApiResponse<AdminUserResponseDto>> CreateAdminUser(AdminUserRegistrationRequestDto request, int requestMaker, string userRoleName);
        Task<ApiResponse<string>> UpdateAdminUser(AdminUserUpdateRequestDto request, int adminId, int userId);
        Task<ApiResponse<string>> DeleteAdminUser(int adminId, int userId);
    }

    public class AdminUserService : IAdminUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminUserService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<ApiResponse<List<AdminUserResponseDto>>> GetAllAdminUsers(PaginationRequest request)
        {
            var skipValue = PaginationUtils.SkipValue(request.Page, request.PageSize);

            var adminUserIds = await _roleManager.Roles
                .Where(x => x.RoleType == UserRoleUtils.GetRoleType(RoleType.admin).ToString())
                .Select(x => x.Id)
                .Distinct()
                .ToListAsync();

            var usersQuery = _unitOfWork.UserRepository.FindAllAsync();
            usersQuery = usersQuery.Where(u => adminUserIds.Contains(u.Id));
            usersQuery = SortUsersQuery(usersQuery, request.SortBy);
            var totalAdminUserCount = await usersQuery.CountAsync();

            var paginatedUsers = await usersQuery
                .Skip(skipValue)
                .Take(request.PageSize)
                .ToListAsync();

            var userIds = paginatedUsers
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

                    return new AdminUserResponseDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        UserRoleName = userRoleName,
                        RoleTypeName = UserRoleUtils.GetRoleType(RoleType.admin).ToString(),
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,

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


            return new ApiPaginationResponse<List<AdminUserResponseDto>>(userListData, request.Page, request.PageSize, totalAdminUserCount);
        }

        public async Task<ApiResponse<AdminUserResponseDto>> GetAdminUserById(int userId)
        {
            var userQuery = _unitOfWork.UserRepository
                .FindByConditionAsync(x => x.Id == userId);

            var user = await userQuery.FirstOrDefaultAsync();
            if (user.HasNoValue())
            {
                return new ApiResponse<AdminUserResponseDto>(null, false, "Admin user not found.");
            }

            var userIds = new List<int> { user.CreatedBy };
            if (user.UpdatedBy.HasValue)
            {
                userIds.Add(user.UpdatedBy.Value);
            }

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var userResponse = await userQuery
                .Select(x => new AdminUserResponseDto
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    Title = x.Title,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,

                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = users.ContainsKey(x.CreatedBy)
                                    ? users[x.CreatedBy].GetFullName()
                                    : "",

                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.UpdatedBy.HasValue && users.ContainsKey(x.UpdatedBy.Value)
                                    ? users[x.UpdatedBy.Value].GetFullName()
                                    : ""
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<AdminUserResponseDto>(userResponse, true, "Admin user found.");
        }

        public async Task<ApiResponse<AdminUserResponseDto>> CreateAdminUser(AdminUserRegistrationRequestDto request, int requestMaker, string userRoleName)
        {
            var allowedRoles = new[]
            {
                UserRoleUtils.GetUserRole(UserRole.developer),
                UserRoleUtils.GetUserRole(UserRole.superadmin)
            };

            if (!allowedRoles.Contains(userRoleName))
            {
                return new ApiResponse<AdminUserResponseDto>(null, false, "Only Developer or Super Admin can create admin users.");
            }

            var validator = new AdminUserRegistrationRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid) 
            {
                return new ApiResponse<AdminUserResponseDto>(validationResult.Errors);
            }

            var isPhoneNumberOrUserExisting = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.PhoneNumber == request.PhoneNumber)
                .FirstOrDefaultAsync();

            if (isPhoneNumberOrUserExisting.HasValue())
            {
                return new ApiResponse<AdminUserResponseDto>(null, false, "Admin with this Phone number already exists in our system.");
            }

            var isEmailExiting = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.Email == request.Email)
                .FirstOrDefaultAsync();

            if (isEmailExiting.HasValue())
            {
                return new ApiResponse<AdminUserResponseDto>(null, false, "Admin with this email already exists in our system.");
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
                return new ApiResponse<AdminUserResponseDto>(userCreationResponse.Errors);
            }

            var userRole  = Enum.Parse<UserRole>(request.UserRoleName, true);

            var roleAssignmentResponse = await _userManager.AddToRoleAsync(user, userRole.ToString());

            if (!roleAssignmentResponse.Succeeded)
            {
                return new ApiResponse<AdminUserResponseDto>(roleAssignmentResponse.Errors);
            }

            return new ApiResponse<AdminUserResponseDto>(null, true, "The Admin user has been created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateAdminUser(AdminUserUpdateRequestDto request, int adminId, int userId)
        {
            var validator = new UpdateAdminUserRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var users = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.Id == adminId || x.Email == request.Email)
                .ToListAsync();

            var user = users.FirstOrDefault(x => x.Id == adminId);
            if (user.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Admin user not found.");
            }

            var isEmailExiting = users.Any(x => x.Id != adminId && x.Email == request.Email); 
            if (isEmailExiting)
            {
                return new ApiResponse<string>(null, false, "Admin with this email already exists.");
            }

            user.Title = request.Title;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Address = request.Address;
            user.UpdatedBy = userId;
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.UserRepository.Update(user);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the admin user.");
            }

            return new ApiResponse<string>(null, true, "The admin user updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteAdminUser(int adminId, int userId)
        {
            var user = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.Id == adminId)
                .FirstOrDefaultAsync();

            if (user.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Admin user not found.");
            }

            if (user.Id == userId) 
            {
                return new ApiResponse<string>(null, false, "You can't delete yourself.");
            }

            user.UpdatedBy = userId;
            user.UpdatedAt = DateTime.Now;
            _unitOfWork.UserRepository.SoftDelete(user);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while deleting the admin user.");
            }

            return new ApiResponse<string>(null, true, "Admin user deleted successfully.");
        }

        #region Helper Methods for Admin User

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
