namespace MedicineShopApplication.DLL.Models
{
    public class Cart : BaseEntity
    {
        public int CartId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
