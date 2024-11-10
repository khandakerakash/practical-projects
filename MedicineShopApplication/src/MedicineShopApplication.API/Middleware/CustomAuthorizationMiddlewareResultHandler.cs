using Microsoft.AspNetCore.Authorization.Policy;
using MedicineShopApplication.BLL.Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace MedicineShopApplication.API.Middleware
{
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
        public async Task HandleAsync(
            RequestDelegate next, 
            HttpContext context, 
            AuthorizationPolicy policy, 
            PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden)
            {
                var response = new ApiResponse<string>(null, false, "403 Forbidded response");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsJsonAsync(response);
                return;
            }

            if (authorizeResult.Challenged)
            {
                var response = new ApiResponse<string>(null, false, "401 Unauthorized response");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(response);
                return;
            }

            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
