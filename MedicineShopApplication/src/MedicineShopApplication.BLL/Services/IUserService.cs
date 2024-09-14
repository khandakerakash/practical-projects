using MedicineShopApplication.BLL.Dtos.Cart;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Dtos.Payment;
using MedicineShopApplication.BLL.Dtos.User;
using MedicineShopApplication.DLL.Models;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

            var usersDto = users
                .Select(u => new UserDto
                {
                    UserDtoId = u.UserId,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    CreatedBy = u.CreatedBy,
                    UpdatedBy = u.UpdatedBy,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,

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
                        OrderDate = o.OrderAt
                    }).ToList(),

                    PaymentsDto = u.Payments?.Select(p => new PaymentDto
                    {
                        PaymentDtoId = p.PaymentId,
                        TransactionId = p.TransactionId,
                        PaymentAmount = p.PaymentAmount,
                        PaymentStatus = p.PaymentStatus,
                        PaymentMethod = p.PaymentMethod,
                        PaymentDate = p.PaymentAt,
                    }).ToList()

                }).ToList();

            return usersDto;    
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.UserId == id).FirstOrDefaultAsync();

            if (user == null) return null;

            var userDto = new UserDto
            {
                UserDtoId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreatedBy = user.CreatedBy,
                UpdatedBy = user.UpdatedBy,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };

            return userDto;
        }

        public async Task<UserDto> AddUser(UserInsertDto userInsertDto)
        {
            if (userInsertDto == null) return null;

            if(userInsertDto.UserName.IsNullOrEmpty())
            {
                throw new Exception("UserName must not be empty");
            }

            if (userInsertDto.FirstName.IsNullOrEmpty())
            {
                throw new Exception("First Name must not be empty");
            }

            if(userInsertDto.Email.IsNullOrEmpty()) 
            {
                throw new Exception("Email must not be empty");
            }

            if (userInsertDto.PhoneNumber.IsNullOrEmpty())
            {
                throw new Exception("Phone Number must not be empty");
            }

            var user = new User
            {
                UserName = userInsertDto.UserName,
                FirstName = userInsertDto.FirstName,
                LastName = userInsertDto.LastName,
                Email = userInsertDto.Email,
                PhoneNumber = userInsertDto.PhoneNumber,
                Address = userInsertDto.Address
            };

            await _unitOfWork.UserRepository.Create(user);

            if (await _unitOfWork.SaveChangesAsync())
            {
                return new UserDto
                {
                    UserName = userInsertDto.UserName,
                    FirstName = userInsertDto.FirstName,
                    LastName = userInsertDto.LastName,
                    Email = userInsertDto.Email,
                    PhoneNumber = userInsertDto.PhoneNumber,
                    Address = userInsertDto.Address
                };
            }

            throw new Exception("Something went wrong, when insert the user.");
        }

        public async Task UpdateUser(UserUpdateDto userUpdateDto)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.UserId == userUpdateDto.UserDtoId).FirstOrDefaultAsync();

            if (user == null || user.IsDeleted) 
            {
                throw new Exception("The user that you want to update has not been found.");
            }

            if(!userUpdateDto.FirstName.IsNullOrEmpty())
            {
                user.FirstName = userUpdateDto.FirstName;
            }

            if(!userUpdateDto.LastName.IsNullOrEmpty())
            {
                user.LastName = userUpdateDto.LastName;
            }

            if(!userUpdateDto.Address.IsNullOrEmpty()) 
            {
                user.Address = userUpdateDto.Address;
            }

            user.UpdatedBy = userUpdateDto.UpdatedBy;
            user.UpdatedAt = userUpdateDto.UpdatedAt;

            _unitOfWork.UserRepository.Update(user);

            if(await _unitOfWork.SaveChangesAsync())
            {
                return;
            }

            throw new Exception("Something went wrong, when updating the user.");
        }

        public async Task DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.UserId == id).FirstOrDefaultAsync();

            if(user == null || user.IsDeleted) 
            { 
                throw new Exception("The user that you want to delete has not been found."); 
            }

            _unitOfWork.UserRepository.Delete(user);

            if(await _unitOfWork.SaveChangesAsync()) 
            { 
                return; 
            }

            throw new Exception("Something went wrong, when deleting the user");
        }
    }
}
