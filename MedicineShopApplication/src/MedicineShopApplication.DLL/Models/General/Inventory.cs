using MedicineShopApplication.DLL.Models.Common;
using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Inventory : AuditableEntity
    {
        public int InventoryId { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public InventoryStatus Status { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }
}
