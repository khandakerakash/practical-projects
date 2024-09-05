namespace MedicineShopApplication.DLL.Models
{
    public class Payment : BaseEntity
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
