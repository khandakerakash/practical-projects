using MedicineShopApplication.BLL.Enums;

namespace MedicineShopApplication.BLL.Utils
{
    public static class UserRoleUtils
    {
        public static string GetUserRoleDisplayName(UserRole userRole)
        {
            return userRole switch
            {
                UserRole.developer => "Developer",
                UserRole.superAdmin => "Super Admin",
                UserRole.admin => "Admin",
                UserRole.manager => "Manager",
                UserRole.moderator => "Moderator",
                UserRole.salesman => "Salesman",
                UserRole.customer => "Customer",
                _ => userRole.ToString()
            };
        }
    }
}
