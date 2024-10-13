namespace MedicineShopApplication.BLL.Dtos.Product
{
    public class CreateProductResponseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }

        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }

        public StrengthDto PowerDto { get; set; }

        public string CreatedByName { get; set; }
    }
}
