using MedicineShopApplication.BLL.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace MedicineShopApplication.API.Policy
{
    public class TokenAuthorizationRequirementPolicy : IAuthorizationRequirement
    {
    }

    public class TokenAuthorizationHandler : AuthorizationHandler<TokenAuthorizationRequirementPolicy>
    {
        private readonly IDistributedCache _cache;

        public TokenAuthorizationHandler(IDistributedCache cache)
        {
            _cache = cache;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenAuthorizationRequirementPolicy requirement)
        {
            if (context.User.Identity.HasValue() && context.User.Identity.IsAuthenticated)
            {
                var username = context.User.FindFirst(x => x.Type == "name")?.Value;
                var key = $"{username}-access-key";
                var redisTokenValue = _cache.GetString(key);

                if (redisTokenValue.HasValue())
                {
                    var getMyTokenValue = context.User.FindFirst(x => x.Type == "my-token-cache-value")?.Value;

                    if (getMyTokenValue.HasValue() && getMyTokenValue == redisTokenValue) 
                    {
                        context.Succeed(requirement); 
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
