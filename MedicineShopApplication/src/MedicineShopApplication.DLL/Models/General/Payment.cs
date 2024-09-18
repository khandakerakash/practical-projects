using MedicineShopApplication.DLL.Models.Common;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Payment : AuditableEntity
    {
        public int PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentAt { get; set; } = DateTime.UtcNow;

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
