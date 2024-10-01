namespace MedicineShopApplication.BLL.Dtos.Base
{
    public class AuditableEntityDto
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
