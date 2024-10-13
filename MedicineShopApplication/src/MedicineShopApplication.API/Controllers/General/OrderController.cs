using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class OrderController : ApiAuthorizeController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("{userId}/place-order")]
        public async Task<IActionResult> PlaceOrder(int userId, [FromBody] CreateOrderRequestDto request)
        {
            var response = await _orderService.PlaceOrder(userId, request);
            return ToActionResult(response);
        }

        [HttpGet("order-details/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var response = await _orderService.GetOrderDetails(orderId);
            return ToActionResult(response);
        }

        [HttpPut("update-payment-status/{orderId}")]
        public async Task<IActionResult> UpdatePaymentStatus(int orderId, [FromBody] string paymentStatus)
        {
            var response = await _orderService.UpdatePaymentStatus(orderId, paymentStatus);
            return ToActionResult(response);
        }
    }
}
