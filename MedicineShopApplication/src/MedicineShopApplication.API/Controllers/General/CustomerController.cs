using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Customer;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class CustomerController : ApiAuthorizeController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomerUser(CustomerUserRegistrationRequestDto request)
        {
            var userRoleName = User.GetUserRole();
            var requestMaker = User.GetUserIdInt();

            var response = await _customerService.CreateCustomerUser(request, userRoleName, requestMaker);
            return ToActionResult(response);
        }
    }
}
