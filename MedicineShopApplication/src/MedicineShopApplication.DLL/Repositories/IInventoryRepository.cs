using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.General;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IInventoryRepository : IRepositoryBase<Inventory>
    {
    }

    public class InventoryRepository : RepositoryBase<Inventory>, IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
