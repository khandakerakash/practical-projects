using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Interfaces;

namespace MedicineShopApplication.DLL.Models.Users
{
    public class ApplicationUser : IdentityUser<int>, IAuditableEntity
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string DateOfBirth { get; set; }
        public string NationalIdentityCard { get; set; }

        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
