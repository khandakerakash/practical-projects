using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.User;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.BLL.Services
{
    public interface IUserService
    {
        Task<ApiResponse<LoggedInUserResponseDto>> GetLoggedInUserInfo(string userRole, string requestMaker);
        Task<ApiResponse<string>> ChangePassword(ChangePasswordRequestDto request, int requestMaker);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse<LoggedInUserResponseDto>> GetLoggedInUserInfo(string userRole, string requestMaker)
        {
            var user = await _unitOfWork.UserRepository
                .FindByConditionAsync(x => x.UserName == requestMaker)
                .FirstOrDefaultAsync();

            if (user.HasNoValue())
            {
                return new ApiResponse<LoggedInUserResponseDto>(null, false, "User not found.");
            }

            var userInfo = new LoggedInUserResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.GetFullName(),
                Role = userRole,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LastLogin = DateTime.UtcNow
            };

            return new ApiResponse<LoggedInUserResponseDto>(userInfo, true, "User found");
        }

        public async Task<ApiResponse<string>> ChangePassword(ChangePasswordRequestDto request, int requestMaker)
        {
            var user = await _unitOfWork.UserRepository
                .FindByConditionWithTrackingAsync(x => x.UserName == request.UserName)
                .FirstOrDefaultAsync();

            if (user.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "User not found.");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new ApiResponse<string>(null, false, "Current password is incorrect.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            user.UpdatedBy = requestMaker;
            user.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.UserRepository.Update(user);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the password.");
            }

            return new ApiResponse<string>(null, true, "Password has been changed successfully.");
        }
    }
}
