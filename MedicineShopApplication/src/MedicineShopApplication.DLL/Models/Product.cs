namespace MedicineShopApplication.DLL.Models
{
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    }
}
