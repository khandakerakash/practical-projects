using MedicineShopApplication.BLL.Dtos.Brand;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Security.Claims;
using MedicineShopApplication.API.Controllers.BasicControllers;

namespace MedicineShopApplication.API.Controllers
{
    public class BrandController : ApiBaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBrand(CreateBrandRequestDto request)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.CreateBrand(request, userId);
            return ToActionResult(response);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPost("create-range")]
        public async Task<IActionResult> CreateBrands(List<CreateBrandRequestDto> requests)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.CreateBrands(requests, userId);
            return ToActionResult(response);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpPut("update/{brandId}")]
        public async Task<IActionResult> UpdateBrand(UpdateBrandRequestDto request, int brandId)
        {
            var userIdString = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            int userId = Convert.ToInt32(userIdString);

            var response = await _brandService.UpdateBrand(request, brandId, userId);
            return ToActionResult(response);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("dropdown-options")]
        public async Task<IActionResult> GetBrandDropdownOptions()
        {
            var response = await _brandService.GetBrandDropdownOptions();
            return ToActionResult(response);
        }
    }
}
