using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface ICartRepository : IRepositoryBase<Cart>
    {
    }

    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
