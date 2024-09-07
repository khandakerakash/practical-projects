using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.CategoryProduct;

namespace MedicineShopApplication.BLL.Dtos.Category
{
    public class CategoryDto : BaseEntityDto
    {
        public int CategoryDtoId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CategoryProductDto> CategoryProductsDto { get; set; }
    }
}
