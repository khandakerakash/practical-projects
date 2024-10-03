using MedicineShopApplication.API.Controllers.BasicControllers;
using MedicineShopApplication.BLL.Dtos.UnitOfMeasure;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers
{
    public class UnitOfMeasureController : ApiAuthorizeBaseController
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public UnitOfMeasureController(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }




        [HttpPost("create")]
        public async Task<IActionResult> CreateUnitOfMeasure(CreateUnitOfMeasureRequestDto request)
        {
            var userId = User.GetUserIdInt();

            var response = await _unitOfMeasureService.CreateUnitOfMeasure(request, userId);
            return ToActionResult(response);
        }
    }
}
