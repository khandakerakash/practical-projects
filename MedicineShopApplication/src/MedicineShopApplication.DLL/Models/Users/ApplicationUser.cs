using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Interfaces;
using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.DLL.Models.Users
{
    public class ApplicationUser : IdentityUser<int>, IAuditableEntity, ISoftDeletable
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NationalIdentityCard { get; set; }
        public string PostalCode { get; set; }
        public string PoliceStation { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Address { get; set; }
        public UserStatus Status { get; set; }

        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
