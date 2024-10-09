namespace MedicineShopApplication.BLL.Dtos.Customer
{
    public class CustomerUserRegistrationRequestDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string UserRoleName { get; set; }
    }
}
