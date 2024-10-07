﻿using MedicineShopApplication.API.Controllers.BasicControllers;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.User;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers
{
    public class AdminUserController : ApiAuthorizeBaseController
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

        [HttpPut("update/{adminId}")]
        public async Task<IActionResult> UpdateAdminUser(UpdateAdminUserRequestDto request, int adminId)
        {
            var userId = User.GetUserIdInt();

            var response = await _adminUserService.UpdateAdminUser(request, adminId, userId);
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
