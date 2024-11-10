using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;


namespace MedicineShopApplication.API.Controllers.General
{
    public class CategoryController : ApiAuthorizeController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
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
            _logger.LogInformation("starting to create new category");

            var userId = User.GetUserIdInt();
            var username = User.GetUserName();

            var response = await _categoryService.CreateCategory(request, userId, username);
            return ToActionResult(response);
        }

        [HttpPost("create-range")]
        public async Task<IActionResult> CreateCategories(List<CreateCategoryRequestDto> requests)
        {
            var userId = User.GetUserIdInt();

            var response = await _categoryService.CreateCategories(requests, userId);
            return ToActionResult(response);
        }

        [HttpPut("update/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequestDto request, int categoryId)
        {
            var userId = User.GetUserIdInt();

            var response = await _categoryService.UpdateCategory(request, categoryId, userId);
            return ToActionResult(response);
        }

        [HttpPost("delete/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var userId = User.GetUserIdInt();

            var response = await _categoryService.DeleteCategory(categoryId, userId);
            return ToActionResult(response);
        }

        [HttpPost("undo/{categoryId}")]
        public async Task<IActionResult> UndoDeletedCategory(int categoryId)
        {
            var userId = User.GetUserIdInt();

            var response = await _categoryService.UndoDeletedCategory(categoryId, userId);
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
