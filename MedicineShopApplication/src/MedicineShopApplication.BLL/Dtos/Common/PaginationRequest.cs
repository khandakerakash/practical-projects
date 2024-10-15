namespace MedicineShopApplication.BLL.Dtos.Common
{
    /// <summary>
    /// Represents a paginated request with sorting capabilities.
    /// </summary>
    public class PaginationRequest
    {
        // Default values
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;

        private int _page = DefaultPage;
        private int _pageSize = DefaultPageSize;

        /// <summary>
        /// Gets or sets the current page number. Defaults to 1.
        /// </summary>
        public int Page
        {
            get => _page;
            set => _page = value > 0 ? value : DefaultPage;
        }

        /// <summary>
        /// Gets or sets the size of the page. Must be between 1 and 50. Defaults to 10.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 0) && (value <= MaxPageSize) ? value : DefaultPageSize;
        }

        /// <summary>
        /// Gets or sets the property name to sort by. Defaults to an empty string.
        /// </summary>
        public string SortBy { get; set; } = string.Empty;

        /// <summary>
        /// Sets the `SearchTerm` for searchable data. Defaults to null.
        /// </summary>
        public string SearchTerm { get; set; } = null;

        /// <summary>
        /// Sets the `Ids` for searchable data. Defaults to an empty list (for multiple IDs) or null (for a single ID).
        /// </summary>
        public List<int> Ids { get; set; } = new List<int>();

    }
}
