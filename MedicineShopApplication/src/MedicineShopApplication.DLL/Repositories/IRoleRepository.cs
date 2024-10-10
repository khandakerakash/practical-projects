using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IRoleRepository : IRepositoryBase<ApplicationRole>
    {
        Task<List<int>> GetUserIdsByRoleIdAsync(int roleId);
        Task<List<int>> GetUserIdsByRoleIdsAsync(List<int> roleIds);
        Task<List<(int UserId, string RoleName)>> GetRolesForUsersAsync(IEnumerable<int> userIds);
    }

    public class RoleRepository : RepositoryBase<ApplicationRole>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<int>> GetUserIdsByRoleIdAsync(int roleId)
        {
            return await _context.UserRoles
                .Where(x => x.RoleId == roleId)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<int>> GetUserIdsByRoleIdsAsync(List<int> roleIds)
        {
            return await _context.UserRoles
                .Where(userRole => roleIds.Contains(userRole.RoleId))
                .Select(userRole => userRole.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<(int UserId, string RoleName)>> GetRolesForUsersAsync(IEnumerable<int> userIds)
        {
            var result = await (from ur in _context.UserRoles
                                join r in _context.Roles on ur.RoleId equals r.Id
                                where userIds.Contains(ur.UserId)
                                select new { ur.UserId, r.Name })
                                .ToListAsync();

            return result.Select(x => (x.UserId, x.Name)).ToList();
        }
    }
}
