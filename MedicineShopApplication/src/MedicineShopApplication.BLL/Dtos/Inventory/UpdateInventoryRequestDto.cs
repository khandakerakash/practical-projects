﻿using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Dtos.Inventory
{
    public class UpdateInventoryRequestDto
    {
        public int ProductId { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string Status { get; set; }
    }
}
