using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface IPaymentRepository : IRepositoryBase<Payment>
    {
    }

    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
