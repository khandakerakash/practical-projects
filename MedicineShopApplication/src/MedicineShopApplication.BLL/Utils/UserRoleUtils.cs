using MedicineShopApplication.BLL.Enums;

namespace MedicineShopApplication.BLL.Utils
{
    public static class UserRoleUtils
    {
        public static string GetUserRole(UserRole userRole)
        {
            return userRole switch
            {
                UserRole.developer => "developer",
                UserRole.superAdmin => "superAdmin",
                UserRole.admin => "admin",
                UserRole.manager => "manager",
                UserRole.moderator => "moderator",
                UserRole.salesman => "salesman",
                UserRole.customer => "customer",
                _ => userRole.ToString()
            };
        }

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
