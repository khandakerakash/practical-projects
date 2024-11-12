using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.DLL.Extension
{
    public static class ApplicationUserExtension
    {
        public static string GetFullName(this ApplicationUser user)
        {
            if (user == null)
            {
                return string.Empty;
            }

            var title = user.Title ?? string.Empty;
            var firstName = user.FirstName ?? string.Empty;
            var lastName = user.LastName ?? string.Empty;

            return $"{title} {firstName} {lastName}".Trim();
        }

        public static async Task<string> GetFullNameByIdAsync(this UserManager<ApplicationUser> userManager, int userId)
        {
            if (userManager == null)
            {
                return string.Empty;
            }

            var user = await userManager.FindByIdAsync(userId.ToString());

            return user?.GetFullName() ?? string.Empty;
        }
    }
}
