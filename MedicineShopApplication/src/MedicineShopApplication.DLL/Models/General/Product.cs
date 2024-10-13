using MedicineShopApplication.DLL.Models.Common;
using MedicineShopApplication.DLL.Models.Enums;
using MedicineShopApplication.DLL.Models.Interfaces;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Product : AuditableEntity, ISoftDeletable
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public ProductStatus Status { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public Strength Power { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class Strength
    {
        public decimal? Amount { get; set; }
        public string Unit { get; set; }
    }
}
