using MedicineShopApplication.BLL.Dtos.Base;

namespace MedicineShopApplication.BLL.Dtos.Category
{
    public class CreateCategoryResponseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string createdByName { get; set; }
    }
}
