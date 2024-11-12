using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Cart;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class CartController : ApiAuthorizeController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartRequestDto request)
        {
            var userId = User.GetUserIdInt();

            var response = await _cartService.AddToCart(userId, request);
            return ToActionResult(response);
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.GetUserIdInt();

            var response = await _cartService.RemoveFromCart(userId, productId);
            return ToActionResult(response);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.GetUserIdInt();

            var response = await _cartService.ClearCart(userId);
            return ToActionResult(response);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetCartDetails()
        {
            var userId = User.GetUserIdInt();

            var response = await _cartService.GetCartDetails(userId);
            return ToActionResult(response);
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateCartItemQuantity(UpdateToCartRequestDto request)
        {
            var userId = User.GetUserIdInt();

            var response = await _cartService.UpdateCartItemQuantity(userId, request);
            return ToActionResult(response);
        }
    }
}
