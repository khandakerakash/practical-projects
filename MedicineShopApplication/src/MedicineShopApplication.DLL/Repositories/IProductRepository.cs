using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.General;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<int> GetProductCountByPrefixAsync(string prefix);
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetProductCountByPrefixAsync(string prefix)
        {
            if (_context.Products != null)
            {
                return await _context.Products
                    .Where(c => c.Code.StartsWith(prefix))
                    .CountAsync();
            }

            return 0;
        }
    }
}
