namespace MedicineShopApplication.BLL.Dtos
{
    public class CartDto : BaseEntityDto
    {
        public int CartDtoId { get; set; }

        public int UserDtoId { get; set; }
        public UserDto UserDto { get; set; }
        public List<CartItemDto> CartItemDtos { get; set; }
    }
}
