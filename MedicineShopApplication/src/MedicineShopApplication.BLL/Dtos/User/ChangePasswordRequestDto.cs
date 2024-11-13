namespace MedicineShopApplication.BLL.Dtos.User
{
    public class ChangePasswordRequestDto
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
