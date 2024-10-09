using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IRoleRepository : IRepositoryBase<ApplicationRole>
    {
    }

    public class RoleRepository : RepositoryBase<ApplicationRole>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
