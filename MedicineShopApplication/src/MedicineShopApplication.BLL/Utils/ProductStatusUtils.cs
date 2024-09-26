using MedicineShopApplication.DLL.Models.Enums;

namespace MedicineShopApplication.BLL.Utils
{
    public static class ProductStatusUtils
    {
        public static string GetProductStatusDisplayName(ProductStatus status)
        {
            return status switch
            {
                ProductStatus.Available => "Available",
                ProductStatus.OutOfStock => "Out of Stock",
                ProductStatus.Discontinued => "Discontinued",
                _ => status.ToString()
            };
        }
    }
}
