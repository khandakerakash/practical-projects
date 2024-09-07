namespace MedicineShopApplication.BLL.Dtos
{
    public class OrderItemDto : BaseEntityDto
    {
        public int OrderItemDtoId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int OrderDtoId { get; set; }
        public OrderDto OrderDto { get; set; }
        public int ProductDtoId { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
