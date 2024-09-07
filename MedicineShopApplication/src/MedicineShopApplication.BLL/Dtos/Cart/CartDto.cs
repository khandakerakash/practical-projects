using MedicineShopApplication.BLL.Dtos.CartItem;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.User;

namespace MedicineShopApplication.BLL.Dtos.Cart
{
    public class CartDto : BaseEntityDto
    {
        public int CartDtoId { get; set; }

        public int UserDtoId { get; set; }
        public UserDto UserDto { get; set; }
        public List<CartItemDto> CartItemDtos { get; set; }
    }
}
