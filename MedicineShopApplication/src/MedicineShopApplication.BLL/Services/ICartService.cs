using MedicineShopApplication.BLL.Dtos.Cart;
using MedicineShopApplication.BLL.Dtos.CartItem;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Customer;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;


namespace MedicineShopApplication.BLL.Services
{
    public interface ICartService
    {
        Task<ApiResponse<string>> AddToCart(int userId, AddToCartRequestDto request);
        Task<ApiResponse<string>> RemoveFromCart(int userId, int productId);
        Task<ApiResponse<string>> ClearCart(int userId);
        Task<ApiResponse<CartResponseDto>> GetCartDetails(int userId);
        Task<ApiResponse<string>> UpdateCartItemQuantity(int userId, UpdateToCartRequestDto request);
    }

    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> AddToCart(int userId, AddToCartRequestDto request)
        {
            var validator = new AddToCartRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var productExists = await _unitOfWork.ProductRepository
                .FindByConditionAsync(x => x.ProductId == request.ProductId)
                .AnyAsync();

            if (!productExists)
            {
                return new ApiResponse<string>(null, false, "Product not found.");
            }

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
                
                if (!await _unitOfWork.CommitAsync() )
                {
                    return new ApiResponse<string>(null, false, "An error occurred while adding the item(s) to the cart.");
                }
            }

            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.ProductId == request.ProductId);

            if (cartItem.HasValue()) 
            {
                cartItem.Quantity += request.Quantity;
                _unitOfWork.CartItemRepository.Update(cartItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    CreatedBy = userId
                };

                await _unitOfWork.CartItemRepository.CreateAsync(newCartItem);
            }

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while adding the item(s) to the cart.");
            }

            return new ApiResponse<string>(null, true, "Item(s) added to the cart successfully.");
        }

        public async Task<ApiResponse<string>> RemoveFromCart(int userId, int productId)
        {
            var productExists = await _unitOfWork.ProductRepository
                .FindByConditionAsync(x => x.ProductId == productId)
                .AnyAsync();

            if (!productExists)
            {
                return new ApiResponse<string>(null, false, "Product not found.");
            }

            var cart = await _unitOfWork.CartRepository
                .FindByConditionWithTrackingAsync(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (cart.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Cart not found.");
            }

            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.ProductId == productId);

            if (cartItem.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Item not found in the cart.");
            }

            _unitOfWork.CartItemRepository.Delete(cartItem);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occured while removing the item form the cart.");
            }

            return new ApiResponse<string>(null, true, "Item removed fromm the cart successfully.");
        }

        public async Task<ApiResponse<string>> ClearCart(int userId)
        {
            var cart = await _unitOfWork.CartRepository
                .FindByConditionWithTrackingAsync(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (cart.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Cart not found.");
            }

            var cartItems = cart.CartItems.ToList();
            _unitOfWork.CartItemRepository.DeleteRange(cartItems);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while clearing the cart.");
            }

            return new ApiResponse<string>(null, true, "Cart cleared successfully.");
        }

        public async Task<ApiResponse<CartResponseDto>> GetCartDetails(int userId)
        {
            var cart = await _unitOfWork.CartRepository
                .FindByConditionAsync(x => x.UserId == userId)
                .Select(x => new CartResponseDto
                {
                    CartId = x.CartId,
                    UserId = x.UserId,
                    CustomerInfoDtos = new CustomerInfoDto
                    {
                        UserId = x.UserId,
                        UserName = x.User.UserName,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        Title = x.User.Title,
                        Email = x.User.Email,
                        PhoneNumber = x.User.PhoneNumber,
                        NationalIdentityCard = x.User.NationalIdentityCard,
                        PostalCode = x.User.PostalCode,
                        PoliceStation = x.User.PoliceStation,
                        District = x.User.District,
                        Division = x.User.Division,
                        Address = x.User.Address,
                    },
                    CartItemResponseDtos = x.CartItems.Select(ci => new CartItemResponseDto
                    {
                        CartId = ci.CartId,
                        CartItemId = ci.CartItemId,
                        Quantity = ci.Quantity,
                        ProductId = ci.ProductId,
                        ProductInfoDtos = new ProductInfoDto
                        {
                            ProductId = ci.ProductId,
                            Name = ci.Product.Name,
                            Code = ci.Product.Code,
                            GenericName = ci.Product.GenericName,
                            Description = ci.Product.Description,
                            SellingPrice = ci.Product.SellingPrice,
                            Status = ProductStatusUtils.GetProductStatusDisplayName(ci.Product.Status),
                            ImageUrl = ci.Product.ImageUrl
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (cart.HasNoValue())
            {
                return new ApiResponse<CartResponseDto>(null, false, "Cart not found.");
            }

            return new ApiResponse<CartResponseDto>(cart, true, "Cart retrieved successfully.");
        }

        public async Task<ApiResponse<string>> UpdateCartItemQuantity(int userId, UpdateToCartRequestDto request)
        {
            var validator = new UpdateToCartRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var productExists = await _unitOfWork.ProductRepository
                .FindByConditionAsync(x => x.ProductId == request.ProductId)
                .AnyAsync();

            if (!productExists)
            {
                return new ApiResponse<string>(null, false, "Product not found.");
            }

            var cart = await _unitOfWork.CartRepository
                .FindByConditionWithTrackingAsync(x => x.UserId == userId)
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync();

            if (cart.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Cart not found.");
            }

            var cartItem = cart.CartItems
                .FirstOrDefault( x => x.ProductId == request.ProductId);

            if (cartItem.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Item not found in the cart.");
            }

            cartItem.Quantity = request.Quantity;
            cartItem.UpdatedBy = userId;
            _unitOfWork.CartItemRepository.Update(cartItem);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the cart item.");
            }

            return new ApiResponse<string>(null, true, "Cart item updated successfully.");
        }
    }
}
