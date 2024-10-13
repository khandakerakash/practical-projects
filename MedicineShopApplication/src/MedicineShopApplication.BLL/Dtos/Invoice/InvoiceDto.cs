using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Order;

namespace MedicineShopApplication.BLL.Dtos.Invoice
{
    public class InvoiceDto : AuditableEntityDto
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceAt { get; set; } = DateTime.UtcNow;

        public int OrderId { get; set; }
        public OrderDto OrderDto { get; set; }
    }
}
