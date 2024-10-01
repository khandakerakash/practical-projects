using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IUserRepository : IRepositoryBase<ApplicationUser>
    {
    }

    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
