namespace MedicineShopApplication.BLL.Dtos.Admin
{
    public class AdminUserUpdateRequestDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NationalIdentityCard { get; set; }
        public string PostalCode { get; set; }
        public string PoliceStation { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Address { get; set; }
    }
}
