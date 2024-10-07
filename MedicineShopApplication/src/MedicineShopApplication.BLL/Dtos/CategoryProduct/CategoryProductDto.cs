using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.BLL.Dtos.Product;

namespace MedicineShopApplication.BLL.Dtos.CategoryProduct
{
    public class CategoryProductDto
    {
        public int CategoryId { get; set; }
        public CategoryDto CategoryDto { get; set; }
        public int ProductDtoId { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
