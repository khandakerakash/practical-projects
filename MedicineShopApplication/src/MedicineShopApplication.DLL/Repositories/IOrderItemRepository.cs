using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IOrderItemRepository : IRepositoryBase<OrderItem>
    {
    }

    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
