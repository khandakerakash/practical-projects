using MedicineShopApplication.DLL.Models.Common;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.DLL.Models.General
{
    public class Cart : AuditableEntity
    {
        public int CartId { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
