using MedicineShopApplication.BLL.Dtos.UnitOfMeasure;
using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;


namespace MedicineShopApplication.API.Controllers.General
{
    public class UnitOfMeasureController : ApiAuthorizeController
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public UnitOfMeasureController(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnitOfMeasure([FromQuery] PaginationRequest request)
        {
            var response = await _unitOfMeasureService.GetAllUnitOfMeasure(request);
            return ToActionResult(response);
        }

        [HttpGet("{unitOfMeasureId}")]
        public async Task<IActionResult> GetUnitOfMeasureById(int unitOfMeasureId)
        {
            var response = await _unitOfMeasureService.GetUnitOfMeasureById(unitOfMeasureId);
            return ToActionResult(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUnitOfMeasure(CreateUnitOfMeasureRequestDto request)
        {
            var userId = User.GetUserIdInt();

            var response = await _unitOfMeasureService.CreateUnitOfMeasure(request, userId);
            return ToActionResult(response);
        }

        [HttpPut("update/{unitOfMeasureId}")]
        public async Task<IActionResult> UpdateUnitOfMeasure(UpdateUnitOfMeasureRequestDto request, int unitOfMeasureId)
        {
            var userId = User.GetUserIdInt();

            var response = await _unitOfMeasureService.UpdateUnitOfMeasure(request, unitOfMeasureId, userId);
            return ToActionResult(response);
        }

        [HttpDelete("delete/{unitOfMeasureId}")]
        public async Task<IActionResult> DeleteUnitOfMeasure(int unitOfMeasureId)
        {
            var userId = User.GetUserIdInt();

            var response = await _unitOfMeasureService.DeleteUnitOfMeasure(unitOfMeasureId, userId);
            return ToActionResult(response);
        }

        [HttpGet("dropdown-options")]
        public async Task<IActionResult> GetUnitOfMeasureDropdownOptions()
        {
            var response = await _unitOfMeasureService.GetUnitOfMeasureDropdownOptions();
            return ToActionResult(response);
        }
    }
}
