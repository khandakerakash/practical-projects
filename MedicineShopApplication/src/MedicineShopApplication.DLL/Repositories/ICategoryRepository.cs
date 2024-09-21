using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.General;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<int> GetCategoryCountByPrefixAsync(string prefix);
    }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the count of categories in the database that have a code starting with the specified prefix.
        /// 
        /// This method queries the database to find how many categories already exist with a code that begins 
        /// with the given prefix. This is typically used to ensure uniqueness when generating new category codes.
        /// 
        /// Example: 
        /// - If the prefix is "PPI", this will return the number of categories whose codes start with "PPI".
        /// 
        /// </summary>
        /// <param name="prefix">The prefix to search for in the category codes.</param>
        /// <returns>An integer representing the count of categories that have codes starting with the given prefix.</returns>

        public async Task<int> GetCategoryCountByPrefixAsync(string prefix)
        {
            if (_context.Categories != null)
            {
                return await _context.Categories
                    .Where(c => c.Code.StartsWith(prefix))
                    .CountAsync();
            }

            return 0;
        }
    }
}
