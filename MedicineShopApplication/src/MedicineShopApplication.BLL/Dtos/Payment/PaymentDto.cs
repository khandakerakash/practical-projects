using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Dtos.User;

namespace MedicineShopApplication.BLL.Dtos.Payment
{
    public class PaymentDto : BaseEntityDto
    {
        public int PaymentDtoId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public int OrderDtoId { get; set; }
        public OrderDto OrderDto { get; set; }
        public int UserDtoId { get; set; }
        public UserDto UserDto { get; set; }
    }
}
