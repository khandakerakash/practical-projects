using Microsoft.AspNetCore.Mvc;
using MedicineShopApplication.BLL.Services;
using MedicineShopApplication.BLL.Dtos.Account;
using MedicineShopApplication.API.Controllers.BasicControllers;

namespace MedicineShopApplication.API.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequestDto request)
        {
            var response = await _accountService.Register(request);
            return ToActionResult(response);
        }
    }
}
