using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Order;

namespace MedicineShopApplication.BLL.Dtos.Invoice
{
    public class InvoiceDto : AuditableEntityDto
    {
        public int InvoiceDtoId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        public int OrderDtoId { get; set; }
        public OrderDto OrderDto { get; set; }
    }
}
