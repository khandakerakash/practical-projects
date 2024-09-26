using MedicineShopApplication.DLL.Models.Common;

namespace MedicineShopApplication.DLL.Models.General
{
    public class UnitOfMeasure : AuditableEntity
    {
        public int UnitOfMeasureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}
