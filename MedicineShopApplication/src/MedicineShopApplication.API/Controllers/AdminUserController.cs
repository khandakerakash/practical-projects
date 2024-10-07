using MedicineShopApplication.API.Controllers.BasicControllers;
using MedicineShopApplication.BLL.Dtos.Common;
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
    }
}
