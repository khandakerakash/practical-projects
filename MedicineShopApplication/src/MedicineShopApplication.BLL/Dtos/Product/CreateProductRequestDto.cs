using MedicineShopApplication.BLL.Dtos.CategoryProduct;

namespace MedicineShopApplication.BLL.Dtos.Product
{
    public class CreateProductRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        public List<CategoryProductDto> CategoryProductsDto { get; set; } = new List<CategoryProductDto>();
    }
}
