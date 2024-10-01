using MedicineShopApplication.BLL.Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.BasicControllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthorizeBaseController : ControllerBase
    {
        public IActionResult ToActionResult<T>(ApiResponse<T> result)
        {
            if (!result.IsSuccess)
            {

                if (result.Errors != null && result.Errors.Count > 0)
                {
                    return UnprocessableEntity(result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
