namespace MedicineShopApplication.DLL.Models
{
    public class Invoice : BaseEntity
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceDate { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
