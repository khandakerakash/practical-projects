
using MedicineShopApplication.DLL.Models.Interfaces;
using MedicineShopApplication.DLL.DbContextInit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> FindAllAsync(bool includeDeleted = false, params Expression<Func<T, object>>[] includes);
        IQueryable<T> FindByConditionAsync(Expression<Func<T, bool>> condition, bool includeDeleted = false);
        IQueryable<T> FindByConditionWithTrackingAsync(Expression<Func<T, bool>> condition, bool includeDeleted = false);
        Task CreateAsync(T entity);
        Task CreateRangeAsync(List<T> entities);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        void Delete(T entity);
        void DeleteRange(List<T> entities);
        void SoftDelete(T entity);
        void UndoSoftDelete(T entity);
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

        public IQueryable<T> FindAllAsync(bool includeDeleted = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)) && !includeDeleted)
            {
                query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.AsNoTracking();
        }

        public IQueryable<T> FindByConditionAsync(Expression<Func<T, bool>> condition, bool includeDeleted = false)
        {
            IQueryable<T> query = _dbSet;

            query = query.Where(condition);

            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)) && !includeDeleted)
            {
                query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));
            }

            return query.AsNoTracking();
        }

        public IQueryable<T> FindByConditionWithTrackingAsync(Expression<Func<T, bool>> condition, bool includeDeleted = false)
        {
            IQueryable<T> query = _dbSet.Where(condition);

            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)) && !includeDeleted)
            {
                query = query.Where(e => !EF.Property<bool>(e, "IsDeleted"));
            }

            return query;
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

        public void SoftDelete(T entity)
        {
            if (entity is ISoftDeletable deletableEntity)
            {
                deletableEntity.IsDeleted = true;
                deletableEntity.DeletedAt = DateTime.Now;
                Update(entity);
            }
        }

        public void UndoSoftDelete(T entity)
        {
            if (entity is ISoftDeletable deletableEntity)
            {
                deletableEntity.IsDeleted = false;
                deletableEntity.DeletedAt = null;
                Update(entity);
            }
        }
    }
}
