using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Customer;
using MedicineShopApplication.BLL.Dtos.Invoice;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Dtos.OrderItem;
using MedicineShopApplication.BLL.Dtos.Payment;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IOrderService
    {
        Task<ApiResponse<string>> PlaceOrder(int userId, CreateOrderRequestDto request);
        Task<ApiResponse<OrderDto>> GetOrderDetails(int orderId);
        Task<ApiResponse<string>> UpdatePaymentStatus(int orderId, string paymentStatus);
    }

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> PlaceOrder(int userId, CreateOrderRequestDto request)
        {
            var validator = new CreateOrderRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            var cart = await _unitOfWork.CartRepository
                .FindByConditionWithTrackingAsync(x => x.UserId == userId)
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync();

            if (cart.HasNoValue() || !cart.CartItems.Any())
            {
                return new ApiResponse<string>(null, false, "Cart is empty or not found.");
            }

            var totalAmount = cart.CartItems.Sum(item => item.Quantity * item.Product.SellingPrice);

            var newOrder = new Order
            {
                UserId = userId,
                DeliveryAddress = request.DeliveryAddress,
                TotalAmount = totalAmount,
                PaymentStatus = "Pending",
                OrderItems = cart.CartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.SellingPrice,
                    Product = item.Product
                }).ToList(),
            };

            await _unitOfWork.OrderRepository.CreateAsync(newOrder);

            _unitOfWork.CartRepository.Delete(cart);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occured while placing the order.");
            }

            return new ApiResponse<string>(newOrder.OrderId.ToString(), true, "Order placed successfully.");
        }

        public async Task<ApiResponse<OrderDto>> GetOrderDetails(int orderId)
        {
            var order = await _unitOfWork.OrderRepository
                .FindByConditionAsync(x => x.OrderId == orderId)
                .Include(x => x.OrderItems)
                .Include(x => x.User)
                //.Include(x => x.Payment)
                //.Include(x => x.Invoice)
                .FirstOrDefaultAsync();

            if (order.HasNoValue())
            {
                return new ApiResponse<OrderDto>(null, false, "Order not found.");
            }

            var orderResponse = new OrderDto
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                PaymentStatus = order.PaymentStatus,
                DeliveryAddress = order.DeliveryAddress,
                OrderItemsDto = order.OrderItems.Select(item => new OrderItemDto
                {
                    OrderItemId = item.OrderItemId,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                }).ToList(),
                CustomerInfoDto = new CustomerInfoDto
                {
                    UserId = order.UserId,
                    UserName = order.User.UserName,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName,
                    Title = order.User.Title,
                    Email = order.User.Email,
                    PhoneNumber = order.User.PhoneNumber,
                    NationalIdentityCard = order.User.NationalIdentityCard,
                    PostalCode = order.User.PostalCode,
                    PoliceStation = order.User.PoliceStation,
                    District = order.User.District,
                    Division = order.User.Division,
                    Address = order.User.Address,
                },
                //PaymentDto = new PaymentDto
                //{
                //    PaymentId = order.Payment.PaymentId,
                //    TransactionId = order.Payment.TransactionId,
                //    PaymentMethod = order.Payment.PaymentMethod,
                //    PaymentAmount = order.Payment.PaymentAmount,
                //    PaymentStatus = order.Payment.PaymentStatus,
                //    OrderId = order.Payment.OrderId,
                //    UserId = order.Payment.UserId,
                //},
                //InvoiceDto = new InvoiceDto
                //{
                //    OrderId = order.Payment.OrderId,
                //    InvoiceId = order.Invoice.InvoiceId,
                //    TotalAmount = order.Invoice.TotalAmount
                //}
            };

            return new ApiResponse<OrderDto>(orderResponse, true, "Order details retrieved successfully.");
        }

        public async Task<ApiResponse<string>> UpdatePaymentStatus(int orderId, string paymentStatus)
        {
            var order = await _unitOfWork.OrderRepository
                .FindByConditionWithTrackingAsync(x => x.OrderId == orderId)
                .FirstOrDefaultAsync();

            if (order.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Order not found.");
            }

            order.PaymentStatus = paymentStatus;
            _unitOfWork.OrderRepository.Update(order);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "Failed to update payment status.");
            }

            return new ApiResponse<string>(null, true, "Payment status updated successfully.");
        }
    }
}
