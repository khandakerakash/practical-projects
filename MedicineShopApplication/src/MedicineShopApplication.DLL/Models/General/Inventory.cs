using MedicineShopApplication.DLL.Models.Common;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Inventory : AuditableEntity
    {
        public int InventoryId { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public string Location { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
