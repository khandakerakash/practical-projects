using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Dtos.Product;

namespace MedicineShopApplication.BLL.Dtos.OrderItem
{
    public class OrderItemDto : AuditableEntityDto
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public OrderDto OrderDto { get; set; }
        public int ProductId { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
