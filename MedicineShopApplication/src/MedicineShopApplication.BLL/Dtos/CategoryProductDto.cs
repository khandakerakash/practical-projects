namespace MedicineShopApplication.BLL.Dtos
{
    public class CategoryProductDto
    {
        public int CategoryDtoId { get; set; }
        public CategoryDto CategoryDto { get; set; }
        public int ProductDtoId { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
