using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.BLL.Services;
using MedicineShopApplication.API.Controllers.BasicControllers;
using MedicineShopApplication.BLL.Dtos.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OpenIddict.Abstractions;


namespace MedicineShopApplication.API.Controllers
{
    public class CategoryController : ApiAuthorizeBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] PaginationRequest request)
        {
            var response = await _categoryService.GetAllCategories(request);
            return ToActionResult(response);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var response = await _categoryService.GetCategoryById(categoryId);
            return ToActionResult(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _categoryService.CreateCategory(request,  userId);
            return ToActionResult(response);
        }

        [HttpPost("create-range")]
        public async Task<IActionResult> CreateCategories(List<CreateCategoryRequestDto> requests)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _categoryService.CreateCategories(requests, userId);
            return ToActionResult(response);
        }

        [HttpPut("update/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequestDto request, int categoryId)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _categoryService.UpdateCategory(request, categoryId, userId);
            return ToActionResult(response);
        }

        [HttpGet("dropdown-options")]
        public async Task<IActionResult> GetCategoryDropdownOptions()
        {
            var response = await _categoryService.GetCategoryDropdownOptions();
            return ToActionResult(response);
        }
    }
}
