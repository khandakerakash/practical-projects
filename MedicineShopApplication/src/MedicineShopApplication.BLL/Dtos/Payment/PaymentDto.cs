using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Order;

namespace MedicineShopApplication.BLL.Dtos.Payment
{
    public class PaymentDto : AuditableEntityDto
    {
        public int PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public int OrderDtoId { get; set; }
        public OrderDto OrderDto { get; set; }
        public int UserDtoId { get; set; }
        public AdminDto UserDto { get; set; }
    }
}
