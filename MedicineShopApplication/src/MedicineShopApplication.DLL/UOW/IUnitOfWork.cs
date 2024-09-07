using MedicineShopApplication.DLL.DbContextInit;

namespace MedicineShopApplication.DLL.UOW
{
    public interface IUnitOfWork : IDisposable
    {

        Task<bool> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _context.Dispose();
                // Dispose others component if needed.
            }
        }
    }
}
