using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Customer;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.DLL.Extension;
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

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers([FromQuery] PaginationRequest request)
        {
            var response = await _customerService.GetAllCustomers(request);
            return ToActionResult(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCustomerById(int userId)
        {
            var response = await _customerService.GetCustomerById(userId);
            return ToActionResult(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCustomerUser(CustomerUserRegistrationRequestDto request)
        {
            var userRoleName = User.GetUserRole();
            var requestMaker = User.GetUserIdInt();

            var response = await _customerService.CreateCustomerUser(request, userRoleName, requestMaker);
            return ToActionResult(response);
        }

        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateCustomerUser(CustomerUserUpdateRequestDto request, int userId)
        {
            var userRoleName = User.GetUserRole();
            var requestMaker = User.GetUserIdInt();

            var response = await _customerService.UpdateCustomerUser(request, userId, requestMaker);
            return ToActionResult(response);
        }
    }
}
