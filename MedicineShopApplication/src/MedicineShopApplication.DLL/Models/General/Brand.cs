using MedicineShopApplication.DLL.Models.Common;
using MedicineShopApplication.DLL.Models.Interfaces;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Brand : AuditableEntity, ISoftDeletable
    {
        public int BrandId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
