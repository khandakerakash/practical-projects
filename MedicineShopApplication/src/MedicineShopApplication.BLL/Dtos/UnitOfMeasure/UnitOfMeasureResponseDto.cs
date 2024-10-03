using MedicineShopApplication.BLL.Dtos.Inventory;
using MedicineShopApplication.BLL.Dtos.Product;

namespace MedicineShopApplication.BLL.Dtos.UnitOfMeasure
{
    public class UnitOfMeasureResponseDto
    {
        public int UnitOfMeasureDtoId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public ICollection<ProductDto> ProductDtos { get; set; } = new List<ProductDto>();
        public ICollection<InventoryDto> InventoryDtos { get; set; } = new List<InventoryDto>();
    }
}
