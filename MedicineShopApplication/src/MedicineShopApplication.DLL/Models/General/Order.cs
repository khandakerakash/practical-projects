using MedicineShopApplication.DLL.Models.Common;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Order : AuditableEntity
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Payment Payment { get; set; }
        public Invoice Invoice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
