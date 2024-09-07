using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineShopApplication.DLL.Repositories
{
    public interface ICartItemRepository : IRepositoryBase<CartItem>
    {
    }

    public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
