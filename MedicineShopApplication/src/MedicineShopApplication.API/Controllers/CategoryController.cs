using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OpenIddict.Abstractions;

namespace MedicineShopApplication.API.Controllers
{
    public class CategoryController : ApiBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _categoryService.CreateCategory(request,  userId);
            return ToActionResult(response);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("create-range")]
        public async Task<IActionResult> CreateCategories(List<CreateCategoryRequestDto> requests)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _categoryService.CreateCategories(requests, userId);
            return ToActionResult(response);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("dropdown-options")]
        public async Task<IActionResult> GetCategoryDropdownOptions()
        {
            var response = await _categoryService.GetCategoryDropdownOptions();
            return ToActionResult(response);
        }
    }
}
