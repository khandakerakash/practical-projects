using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models.General;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IUnitOfMeasureRepository : IRepositoryBase<UnitOfMeasure>
    {
    }

    public class UnitOfMeasureRepository : RepositoryBase<UnitOfMeasure>, IUnitOfMeasureRepository
    {
        private readonly ApplicationDbContext _context;

        public UnitOfMeasureRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
