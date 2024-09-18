using MedicineShopApplication.DLL.DbContextInit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> FindAllAsync(params Expression<Func<T, object>>[] includes);
        IQueryable<T> FindByConditionAsync(Expression<Func<T, bool>> condition);
        Task CreateAsync(T entity);
        Task CreateRangeAsync(List<T> entities);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        void Delete(T entity);
        void DeleteRange(List<T> entities);
    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class 
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> FindAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            // Apply each Include() specified in the parameters
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public IQueryable<T> FindByConditionAsync(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Where(condition).AsNoTracking();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task CreateRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
