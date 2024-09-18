using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Product;

namespace MedicineShopApplication.BLL.Dtos.Category
{
    public class CreateCategoryRequestDto : AuditableEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductDto> ProductDtos { get; set; } = new List<ProductDto>();
    }
}
