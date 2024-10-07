using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Utils
{
    public static class InventoryStatusUtils
    {
        public static string GetInventoryStatusDisplayName(InventoryStatus status)
        {
            return status switch
            {
                InventoryStatus.Available => "Available",
                InventoryStatus.OutOfStock => "Out of Stock",
                InventoryStatus.Reordered => "Reordered",
                _ => status.ToString()
            };
        }
    }
}
