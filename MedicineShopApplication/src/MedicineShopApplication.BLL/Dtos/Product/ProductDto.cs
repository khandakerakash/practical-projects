using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.CartItem;
using MedicineShopApplication.BLL.Dtos.CategoryProduct;
using MedicineShopApplication.BLL.Dtos.OrderItem;

namespace MedicineShopApplication.BLL.Dtos.Product
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

        public List<CategoryProductDto> CategoryProductsDto { get; set; }
        public List<CartItemDto> CartItemsDto { get; set; }
        public List<OrderItemDto> OrderItemsDto { get; set; }
    }
}
