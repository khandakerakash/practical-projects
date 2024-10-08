using MedicineShopApplication.BLL.Dtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
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
