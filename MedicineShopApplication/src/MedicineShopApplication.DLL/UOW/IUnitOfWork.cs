using MedicineShopApplication.DLL.DbContextInit;
using MedicineShopApplication.DLL.Repositories;

namespace MedicineShopApplication.DLL.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICartRepository CartRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }

        Task<bool> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private IUserRepository _userRepository;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ICartRepository _cartRepository;
        private ICartItemRepository _cartItemRepository;
        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;
        private IPaymentRepository _paymentRepository;
        private IInvoiceRepository _invoiceRepository;

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);

        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);

        public ICartRepository CartRepository => _cartRepository ??= new CartRepository(_context);

        public ICartItemRepository CartItemRepository => _cartItemRepository ??= new CartItemRepository(_context);

        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context);

        public IOrderItemRepository OrderItemRepository => _orderItemRepository ??= new OrderItemRepository(_context);

        public IPaymentRepository PaymentRepository => _paymentRepository ??= new PaymentRepository(_context);

        public IInvoiceRepository InvoiceRepository => _invoiceRepository ??= new InvoiceRepository(_context);


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
