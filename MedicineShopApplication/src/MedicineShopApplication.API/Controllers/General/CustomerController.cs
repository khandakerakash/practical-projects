using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Common;
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
    }
}
