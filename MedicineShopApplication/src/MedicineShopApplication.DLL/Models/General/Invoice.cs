using MedicineShopApplication.DLL.Models.Common;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Invoice : AuditableEntity
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceAt { get; set; } = DateTime.UtcNow;

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
