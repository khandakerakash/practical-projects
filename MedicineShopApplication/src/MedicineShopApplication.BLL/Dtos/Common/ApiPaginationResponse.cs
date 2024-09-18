namespace MedicineShopApplication.BLL.Dtos.Common
{
    /// <summary>
    /// Represents a paginated API response.
    /// </summary>
    /// <typeparam name="T">The type of the data returned in the response.</typeparam>
    public class ApiPaginationResponse<T> : ApiResponse<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiPaginationResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data returned in the response.</param>
        /// <param name="currentPage">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="totalCount">The total count of items.</param>
        public ApiPaginationResponse(T date, int currentPage, int pageSize, int totalCount) : base(date)
        {
            CurrentPage = currentPage > 0 ? currentPage : 1;
            PageSize = pageSize > 0 ? pageSize : 10;
            TotalCount = totalCount >= 0 ? totalCount : 0;
            TotalPages = (int)Math.Ceiling((double)TotalCount / Math.Max(pageSize, 1));
        }
    }
}
