using MedicineShopApplication.BLL.Dtos.Base;

namespace MedicineShopApplication.BLL.Dtos.Category
{
    public class CategoryResponseDto : AuditableEntityDto
    {
        public int CategoryDtoId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
    }
}
