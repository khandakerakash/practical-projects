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

        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UnitOfMeasureId { get; set; }

        public StrengthDto Power { get; set; }
    }
}
