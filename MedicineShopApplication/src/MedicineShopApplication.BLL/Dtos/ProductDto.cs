namespace MedicineShopApplication.BLL.Dtos
{
    public class ProductDto : BaseEntityDto
    {
        public int ProductDtoId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        public List<CategoryProductDto> CategoryProductDtos { get; set; }
        public List<CartItemDto> CartItemDtos { get; set; }
        public List<OrderItemDto> OrderItemDtos { get; set; }
    }
}
