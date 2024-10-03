using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.BLL.Dtos.CategoryProduct;

namespace MedicineShopApplication.BLL.Dtos.Product
{
    public class CreateProductRequestDto
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

        public int BrandDtoId { get; set; }
        public int CategoryDtoId { get; set; }
        public int UnitOfMeasureDtoId { get; set; }
    }
}
