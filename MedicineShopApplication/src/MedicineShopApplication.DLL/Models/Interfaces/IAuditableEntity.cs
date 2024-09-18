namespace MedicineShopApplication.DLL.Models.Interfaces
{
    public interface IAuditableEntity
    {
        int? CreatedBy { get; set; }
        int? LastUpdatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? LastModifiedAt { get; set; }
    }
}
