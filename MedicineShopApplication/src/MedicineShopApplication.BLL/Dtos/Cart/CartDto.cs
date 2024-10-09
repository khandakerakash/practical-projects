using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.CartItem;

namespace MedicineShopApplication.BLL.Dtos.Cart
{
    public class CartDto : AuditableEntityDto
    {
        public int CartId { get; set; }

        public int UserDtoId { get; set; }
        public AdminDto UserDto { get; set; }
        public List<CartItemDto> CartItemsDto { get; set; }
    }
}
