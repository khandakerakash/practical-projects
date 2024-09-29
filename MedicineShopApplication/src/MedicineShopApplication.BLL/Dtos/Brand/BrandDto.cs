using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.DLL.Models.Common;

namespace MedicineShopApplication.BLL.Dtos.Brand
{
    public class BrandDto : AuditableEntity
    {
        public int BrandId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public ICollection<ProductDto> ProductDtos { get; set; } = new List<ProductDto>();
    }
}
