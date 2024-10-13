namespace MedicineShopApplication.BLL.Dtos.Inventory
{
    public class InventoryResponseDto
    {
        public int InventoryId { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
    }
}
