namespace MedicineShopApplication.DLL.Models.Interfaces
{
    public interface IAuditableEntity
    {
        int CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        int? UpdatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
