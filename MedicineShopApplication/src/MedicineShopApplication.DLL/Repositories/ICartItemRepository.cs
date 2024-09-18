using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.General;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface ICartItemRepository : IRepositoryBase<CartItem>
    {
    }

    public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
