using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Utils
{
    public static class UserStatusUtils
    {
        public static string GetUserStatus(UserStatus status)
        {
            return status switch
            {
                UserStatus.Active => "Active",
                UserStatus.Inactive => "Inactive",
                UserStatus.Pending => "Pending",
                UserStatus.Banned => "Banned",
                UserStatus.Suspended => "Suspended",
                _ => status.ToString()
            };
        }
    }
}
