using MedicineShopApplication.BLL.Dtos.Cart;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Dtos.Payment;
using MedicineShopApplication.BLL.Dtos.User;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
        Task<UserDto> AddUser(UserInsertDto userInsertDto);
        Task UpdateUser(UserUpdateDto userUpdateDto);
        Task DeleteUser(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _unitOfWork.UserRepository.FindAll()
                .Include(u => u.Cart)
                .Include(u => u.Orders)
                .Include(u => u.Payments)
                .ToListAsync();

            var usersDto = users.Where(u => !u.IsDeleted)
                .Select(u => new UserDto
                {
                    UserDtoId = u.UserId,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,

                    CartDto = u.Cart == null ? null : new CartDto
                    {
                        CartDtoId = u.Cart.CartId,
                    },

                    OrdersDto = u.Orders?.Select(o => new OrderDto
                    {
                        OrderDtoId = o.OrderId,
                        TotalAmount = o.TotalAmount,
                        PaymentStatus = o.PaymentStatus,
                        DeliveryAddress = o.DeliveryAddress,
                        OrderDate = o.OrderDate
                    }).ToList(),

                    PaymentsDto = u.Payments?.Select(p => new PaymentDto
                    {
                        PaymentDtoId = p.PaymentId,
                        TransactionId = p.TransactionId,
                        PaymentAmount = p.PaymentAmount,
                        PaymentStatus = p.PaymentStatus,
                        PaymentMethod = p.PaymentMethod,
                        PaymentDate = p.PaymentDate,
                    }).ToList()

                }).ToList();

            return usersDto;    
        }

        public Task<UserDto> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> AddUser(UserInsertDto userInsertDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(UserUpdateDto userUpdateDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
