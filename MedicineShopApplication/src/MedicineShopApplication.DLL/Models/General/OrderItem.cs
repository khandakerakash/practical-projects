using MedicineShopApplication.DLL.Models.Common;

namespace MedicineShopApplication.DLL.Models.General
{
    public class OrderItem : AuditableEntity
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
