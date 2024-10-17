using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class AdminUserController : AdminAuthorizeController
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdminUsers([FromQuery] PaginationRequest request)
        {
            var response = await _adminUserService.GetAllAdminUsers(request);
            return ToActionResult(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAdminUserById(int userId)
        {
            var response = await _adminUserService.GetAdminUserById(userId);
            return ToActionResult(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAdminUser(AdminUserRegistrationRequestDto request)
        {
            var requestMaker = User.GetUserIdInt();
            var userRoleName = User.GetUserRole();

            var response = await _adminUserService.CreateAdminUser(request, requestMaker, userRoleName);
            return ToActionResult(response);
        }

        [HttpPut("update/{adminId}")]
        public async Task<IActionResult> UpdateAdminUser(AdminUserUpdateRequestDto request, int adminId)
        {
            var userId = User.GetUserIdInt();

            var response = await _adminUserService.UpdateAdminUser(request, adminId, userId);
            return ToActionResult(response);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateAdminUserStatus(AdminUserStatusUpdateRequestDto request)
        {
            var requestMaker = User.GetUserIdInt();

            var response = await _adminUserService.UpdateAdminUserStatus(request, requestMaker);
            return ToActionResult(response);
        }

        [HttpPost("delete/{adminId}")]
        public async Task<IActionResult> DeleteAdminUser(int adminId)
        {
            var userId = User.GetUserIdInt();

            var response = await _adminUserService.DeleteAdminUser(adminId, userId);
            return ToActionResult(response);
        }


    }
}
