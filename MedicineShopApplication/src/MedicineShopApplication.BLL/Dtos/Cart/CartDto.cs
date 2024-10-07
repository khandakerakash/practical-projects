using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.CartItem;
using MedicineShopApplication.BLL.Dtos.User;

namespace MedicineShopApplication.BLL.Dtos.Cart
{
    public class CartDto : AuditableEntityDto
    {
        public int CartId { get; set; }

        public int UserDtoId { get; set; }
        public UserDto UserDto { get; set; }
        public List<CartItemDto> CartItemsDto { get; set; }
    }
}
