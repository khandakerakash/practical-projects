using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.DLL.Models.Users
{
    public class UserStatusChangeLog
    {
        public int UserStatusChangeLogId { get; set; }
        public UserStatus OldStatus { get; set; }
        public UserStatus NewStatus { get; set; }
        public string ReasonForChange { get; set; }
        public int ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
