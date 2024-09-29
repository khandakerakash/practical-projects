using MedicineShopApplication.DLL.Models.Common;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Brand : AuditableEntity
    {
        public int BrandId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
