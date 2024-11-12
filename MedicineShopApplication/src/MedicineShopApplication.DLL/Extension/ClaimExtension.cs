using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MedicineShopApplication.DLL.Extension
{
    public static class ClaimExtension
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(c => c.Type == Claims.Subject)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(c => c.Type == Claims.Name)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static string GetTokenValue(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(c => c.Type == "my-token-cache-value")
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static int GetUserIdInt(this ClaimsPrincipal principal)
        {
            var userId = principal.Claims.Where(c => c.Type == Claims.Subject)
                .Select(c => c.Value)
                .FirstOrDefault();

            return Convert.ToInt32(userId);
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(c => c.Type == Claims.Role)
                .Select(c => c.Value)
                .FirstOrDefault();
        }
    }
}
