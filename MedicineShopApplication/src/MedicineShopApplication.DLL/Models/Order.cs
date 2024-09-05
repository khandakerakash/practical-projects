namespace MedicineShopApplication.DLL.Models
{
    public class Order : BaseEntity
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Invoice Invoice { get; set; }
    }
}
