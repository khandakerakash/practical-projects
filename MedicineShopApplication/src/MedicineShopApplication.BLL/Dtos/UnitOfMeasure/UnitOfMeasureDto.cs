using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.BLL.Dtos.Inventory;

namespace MedicineShopApplication.BLL.Dtos.UnitOfMeasure
{
    public class UnitOfMeasureDto : AuditableEntityDto
    {
        public int UnitOfMeasureDtoId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public ICollection<ProductDto> ProductDtos { get; set; } = new List<ProductDto>();
        public ICollection<InventoryDto> InventoryDtos { get; set; } = new List<InventoryDto>();
    }
}
