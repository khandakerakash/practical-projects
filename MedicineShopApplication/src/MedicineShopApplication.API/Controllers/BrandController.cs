using MedicineShopApplication.API.Controllers.BasicControllers;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Brand;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Security.Claims;


namespace MedicineShopApplication.API.Controllers
{
    public class BrandController : ApiAuthorizeBaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands([FromQuery] PaginationRequest request)
        {
            var response = await _brandService.GetAllBrands(request);
            return ToActionResult(response);
        }

        [HttpGet("{brandId}")]
        public async Task<IActionResult> GetBrandById(int brandId)
        {
            var response = await _brandService.GetBrandById(brandId);
            return ToActionResult(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBrand(CreateBrandRequestDto request)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.CreateBrand(request, userId);
            return ToActionResult(response);
        }

        [HttpPost("create-range")]
        public async Task<IActionResult> CreateBrands(List<CreateBrandRequestDto> requests)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.CreateBrands(requests, userId);
            return ToActionResult(response);
        }

        [HttpPut("update/{brandId}")]
        public async Task<IActionResult> UpdateBrand(UpdateBrandRequestDto request, int brandId)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.UpdateBrand(request, brandId, userId);
            return ToActionResult(response);
        }

        [HttpPost("delete/{brandId}")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.DeleteBrand(brandId, userId);
            return ToActionResult(response);
        }

        [HttpPost("undo/{brandId}")]
        public async Task<IActionResult> UndoDeletedCategory(int brandId)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.UndoDeletedBrand(brandId, userId);
            return ToActionResult(response);
        }

        [HttpGet("dropdown-options")]
        public async Task<IActionResult> GetBrandDropdownOptions()
        {
            var response = await _brandService.GetBrandDropdownOptions();
            return ToActionResult(response);
        }
    }
}
