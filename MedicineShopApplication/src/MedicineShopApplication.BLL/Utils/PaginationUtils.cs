namespace MedicineShopApplication.BLL.Utils
{
    /// <summary>
    /// Utility class for handling pagination calculations.
    /// </summary>
    public static class PaginationUtils
    {
        /// <summary>
        /// Calculates the number of items to skip based on the current page and page size.
        /// </summary>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>The number of items to skip.</returns>
        public static int SkipValue(int page, int pageSize)
        {
            // Ensure page is at least 1 and pageSize is positive to avoid negative skip values
            return (Math.Max(page, 1) - 1) * Math.Max(pageSize, 1);
        }
    }
}
