using MedicineShopApplication.API.Controllers.BasicControllers;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers
{
    public class ProductController : ApiAuthorizeBaseController
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

        [HttpPut("update")]
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
