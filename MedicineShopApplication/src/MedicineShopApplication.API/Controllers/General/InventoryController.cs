using MedicineShopApplication.API.Controllers.Base;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Inventory;
using MedicineShopApplication.DLL.Extension;
using MedicineShopApplication.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicineShopApplication.API.Controllers.General
{
    public class InventoryController : ApiAuthorizeController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService) 
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventories([FromQuery] PaginationRequest request)
        {
            var response = await _inventoryService.GetAllInventories(request);
            return ToActionResult(response);
        }

        [HttpGet("{inventoryId}")]
        public async Task<IActionResult> GetInventoryById(int inventoryId)
        {
            var response = await _inventoryService.GetInventoryById(inventoryId);
            return ToActionResult(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateInventory(CreateInventoryRequestDto request)
        {
            var requestMaker = User.GetUserIdInt();

            var response = await _inventoryService.CreateInventory(requestMaker, request);
            return ToActionResult(response);
        }

        [HttpPut("update/{inventoryId}")]
        public async Task<IActionResult> UpdateInventory(int inventoryId, UpdateInventoryRequestDto request)
        {
            var requestMaker = User.GetUserIdInt();

            var response = await _inventoryService.UpdateInventory(requestMaker, inventoryId, request);
            return ToActionResult(response);
        }
    }
}
