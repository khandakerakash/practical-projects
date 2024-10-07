namespace MedicineShopApplication.BLL.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}
