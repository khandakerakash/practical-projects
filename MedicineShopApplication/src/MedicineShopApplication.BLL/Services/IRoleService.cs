using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Enums;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IRoleService
    {
        Task<ApiResponse<List<DropdownOptionDto>>> GetAdminUserRoleDropdownOptions();
    }

    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<List<DropdownOptionDto>>> GetAdminUserRoleDropdownOptions()
        {
            var adminUserRoles = await _unitOfWork.RoleRepository
                .FindByConditionAsync(x => x.RoleType == UserRoleUtils.GetRoleType(RoleType.admin).ToString())
                .Select(x => new DropdownOptionDto
                {
                    Id = x.Id,
                    IdString = x.Name,
                    Value = UserRoleUtils.GetUserRoleDisplayName(Enum.Parse<UserRole>(x.Name, true))
                })
                .ToListAsync();

            return new ApiResponse<List<DropdownOptionDto>>(adminUserRoles, true, "Admin role found.");
        }
    }
}
