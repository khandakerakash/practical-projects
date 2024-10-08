using MedicineShopApplication.API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class RoleController : ApiAuthorizeController
    {
        public RoleController() { }

        [HttpGet("admin/dropdown-options")]
        public async Task<IActionResult> GetAdminRoleDropdownOptions()
        {
            return null;
        }
    }
}
