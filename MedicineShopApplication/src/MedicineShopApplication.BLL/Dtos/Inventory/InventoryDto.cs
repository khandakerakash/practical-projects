using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.DLL.Models.Enums;
using MedicineShopApplication.BLL.Dtos.UnitOfMeasure;

namespace MedicineShopApplication.BLL.Dtos.Inventory
{
    public class InventoryDto : AuditableEntityDto
    {
        public int InventoryId { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public InventoryStatus Status { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }

        public int ProductDtoId { get; set; } 
        public ProductDto ProductDto { get; set; }

        public int UnitOfMeasureDtoId { get; set; }
        public UnitOfMeasureDto UnitOfMeasureDto { get; set; }
    }
}
