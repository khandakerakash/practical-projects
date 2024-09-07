namespace MedicineShopApplication.BLL.Dtos
{
    public class CategoryDto : BaseEntityDto
    {
        public int CategoryDtoId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CategoryProductDto> CategoryProductDtos { get; set; }
    }
}
