using MedicineShopApplication.BLL.Dtos.Base;

namespace MedicineShopApplication.BLL.Dtos.Admin
{
    public class AdminUserResponseDto : AuditableEntityDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserRoleName { get; set; }
        public string RoleTypeName { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
    }
}
