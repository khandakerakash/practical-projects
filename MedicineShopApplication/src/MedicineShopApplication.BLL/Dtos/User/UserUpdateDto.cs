namespace MedicineShopApplication.BLL.Dtos.User
{
    public class UserUpdateDto
    {
        public int UserDtoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
