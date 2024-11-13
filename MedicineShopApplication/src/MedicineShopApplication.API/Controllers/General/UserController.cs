using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.User;
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

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            var requestMaker = User.GetUserIdInt();

            var response = await _userService.ChangePassword(request, requestMaker);
            return ToActionResult(response);
        }
    }
}
