using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
    }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
