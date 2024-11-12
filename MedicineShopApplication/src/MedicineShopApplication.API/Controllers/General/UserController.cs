using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class UserController : ApiAuthorizeController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetLoggedInUserInfo()
        {
            var userRole = User.GetUserRole();
            var requestMaker = User.GetUserName();

            var response = await _userService.GetLoggedInUserInfo(userRole, requestMaker);
            return ToActionResult(response);
        }
    }
}
