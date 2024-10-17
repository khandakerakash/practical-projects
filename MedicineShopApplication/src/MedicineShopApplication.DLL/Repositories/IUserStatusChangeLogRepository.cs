using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.Users;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IUserStatusChangeLogRepository : IRepositoryBase<UserStatusChangeLog>
    {
    }

    public class UserStatusChangeLogRepository : RepositoryBase<UserStatusChangeLog>, IUserStatusChangeLogRepository
    {
        private readonly ApplicationDbContext _context;

        public UserStatusChangeLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
