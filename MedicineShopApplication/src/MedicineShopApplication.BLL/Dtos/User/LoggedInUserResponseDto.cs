namespace MedicineShopApplication.BLL.Dtos.User
{
    public class LoggedInUserResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
