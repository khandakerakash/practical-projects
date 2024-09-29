using MedicineShopApplication.BLL.Dtos.CategoryProduct;

namespace MedicineShopApplication.BLL.Dtos.Product
{
    public class CreateProductResponseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public string Notes { get; set; }
        public string CreatedByName { get; set; }

        public List<CategoryProductDto> CategoryProductsDto { get; set; } = new List<CategoryProductDto>();
    }
}
