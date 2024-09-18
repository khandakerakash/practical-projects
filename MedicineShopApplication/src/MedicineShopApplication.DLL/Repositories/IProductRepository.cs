using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.General;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
