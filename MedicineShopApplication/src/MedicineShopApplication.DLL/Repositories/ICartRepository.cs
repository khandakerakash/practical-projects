using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface ICartRepository : IRepositoryBase<Cart>
    {
    }

    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
