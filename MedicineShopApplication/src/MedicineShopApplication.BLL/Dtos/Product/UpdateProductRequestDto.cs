namespace MedicineShopApplication.BLL.Dtos.Product
{
    public class UpdateProductRequestDto
    {
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public int BrandDtoId { get; set; }
        public int CategoryDtoId { get; set; }
        public int UnitOfMeasureDtoId { get; set; }
    }
}
