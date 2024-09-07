namespace MedicineShopApplication.BLL.Dtos.User
{
    public class UserInsertDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CreatedBy { get; set; }
    }
}
