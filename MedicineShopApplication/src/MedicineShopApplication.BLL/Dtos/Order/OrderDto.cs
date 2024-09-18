using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Invoice;
using MedicineShopApplication.BLL.Dtos.OrderItem;
using MedicineShopApplication.BLL.Dtos.Payment;
using MedicineShopApplication.BLL.Dtos.User;

namespace MedicineShopApplication.BLL.Dtos.Order
{
    public class OrderDto : AuditableEntityDto
    {
        public int OrderDtoId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public List<OrderItemDto> OrderItemsDto { get; set; }
        public int UserDtoId { get; set; }
        public UserDto UserDto { get; set; }
        public PaymentDto PaymentDto { get; set; }
        public InvoiceDto InvoiceDto { get; set; }
    }
}
