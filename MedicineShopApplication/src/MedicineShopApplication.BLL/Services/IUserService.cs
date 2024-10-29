using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.User;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IUserService
    {
        Task<ApiResponse<LoggedInUserResponseDto>> GetLoggedInUserInfo(string userRole, string requestMaker);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
