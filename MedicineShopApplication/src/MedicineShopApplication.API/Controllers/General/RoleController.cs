using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class RoleController : ApiAuthorizeController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("admin/dropdown-options")]
        public async Task<IActionResult> GetAdminRoleDropdownOptions()
        {
            var response = await _roleService.GetAdminUserRoleDropdownOptions();
            return ToActionResult(response);
        }
    }
}
