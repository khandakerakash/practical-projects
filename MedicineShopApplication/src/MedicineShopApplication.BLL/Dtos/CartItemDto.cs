namespace MedicineShopApplication.BLL.Dtos
{
    public class CartItemDto : BaseEntityDto
    {
        public int CartItemDtoId { get; set; }
        public int Quantity { get; set; }

        public int CartDtoId { get; set; }
        public CartDto CartDto { get; set; }
        public int ProductDtoId { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
