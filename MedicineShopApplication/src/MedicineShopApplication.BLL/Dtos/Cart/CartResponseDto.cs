using MedicineShopApplication.BLL.Dtos.CartItem;
using MedicineShopApplication.BLL.Dtos.Customer;

namespace MedicineShopApplication.BLL.Dtos.Cart
{
    public class CartResponseDto
    {
        public int CartId { get; set; }

        public int UserId { get; set; }
        public CustomerInfoDto CustomerInfoDtos { get; set; }
        public List<CartItemResponseDto> CartItemResponseDtos { get; set; }
    }
}
