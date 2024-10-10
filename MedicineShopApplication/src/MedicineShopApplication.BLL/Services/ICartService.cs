using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICartService
    {
        Task<ApiResponse<string>> AddToCartAsync(int userId, int productId, int quantity);
    }

    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await _unitOfWork.CartRepository
                .FindByConditionAsync(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (cart.HasNoValue())
            {
                cart = new Cart
                {
                    UserId = userId
                };

                await _unitOfWork.CartRepository.CreateAsync(cart);
                await _unitOfWork.CommitAsync();
            }

            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.ProductId == productId);

            if (cartItem.HasNoValue()) 
            {
                cartItem.Quantity += quantity;
                _unitOfWork.CartItemRepository.Update(cartItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };

                await _unitOfWork.CartItemRepository.CreateAsync(newCartItem);
            }

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while adding the item(s) to the cart.");
            }

            return new ApiResponse<string>(null, true, "Item(s) added to the cart successfully.");
        }
    }
}
