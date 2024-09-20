﻿using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers
{
    public class CategoryController : ApiBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var response = await _categoryService.CreateCategory(request);
            return ToActionResult(response);
        }
    }
}
