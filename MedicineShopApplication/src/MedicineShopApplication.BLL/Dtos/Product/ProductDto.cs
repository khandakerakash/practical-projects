using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.CartItem;
using MedicineShopApplication.BLL.Dtos.OrderItem;
using MedicineShopApplication.BLL.Dtos.CategoryProduct;

namespace MedicineShopApplication.BLL.Dtos.Product
{
    public class ProductDto : AuditableEntityDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }

        public List<CategoryProductDto> CategoryProductsDto { get; set; }
        public List<CartItemDto> CartItemsDto { get; set; }
        public List<OrderItemDto> OrderItemsDto { get; set; }
    }
}
