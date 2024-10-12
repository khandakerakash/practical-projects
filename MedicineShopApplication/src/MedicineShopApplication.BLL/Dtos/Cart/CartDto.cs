using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.CartItem;
using MedicineShopApplication.BLL.Dtos.Customer;

namespace MedicineShopApplication.BLL.Dtos.Cart
{
    public class CartDto : AuditableEntityDto
    {
        public int CartId { get; set; }

        public int UserId { get; set; }
        public CustomerUserDto CustomerUserDtos { get; set; }
        public List<CartItemDto> CartItemDtos { get; set; }
    }
}
