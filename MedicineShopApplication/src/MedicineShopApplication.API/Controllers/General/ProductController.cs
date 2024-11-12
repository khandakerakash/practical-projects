using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class ProductController : ApiAuthorizeController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] PaginationRequest request)
        {
            var response = await _productService.GetAllProducts(request);
            return ToActionResult(response);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetAProduct(int productId)
        {
            var response = await _productService.GetAProduct(productId);
            return ToActionResult(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(CreateProductRequestDto request)
        {
            var userId = User.GetUserIdInt();

            var response = await _productService.CreateProduct(request, userId);
            return ToActionResult(response);
        }

        [HttpPost("create-range")]
        public async Task<IActionResult> CreateProducts(List<CreateProductRequestDto> requests)
        {
            var userId = User.GetUserIdInt();

            var response = await _productService.CreateProducts(requests, userId);
            return ToActionResult(response);
        }

        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequestDto request, int productId)
        {
            var userId = User.GetUserIdInt();

            var response = await _productService.UpdateProduct(request, productId, userId);
            return ToActionResult(response);
        }

        [HttpPost("delete/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var userId = User.GetUserIdInt();

            var response = await _productService.DeleteProduct(productId, userId);
            return ToActionResult(response);
        }
    }
}
