namespace MedicineShopApplication.BLL.Dtos.Admin
{
    public class AdminUserStatusUpdateRequestDto
    {
        public string UserName { get; set; }
        public string Status { get; set; }
        public string ReasonForChange { get; set; }
    }
}
