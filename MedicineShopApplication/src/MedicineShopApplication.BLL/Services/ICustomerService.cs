using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Customer;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICustomerService
    {
        Task<ApiResponse<List<CustomerUserResponseDto>>> GetAllCustomers(PaginationRequest request);
        Task<ApiResponse<CustomerUserResponseDto>> GetCustomerById(int userId);
        Task<ApiResponse<CustomerUserResponseDto>> CreateCustomer(CustomerUserRegistrationRequestDto request, string userRoleName, int requestMaker);
        Task<ApiResponse<string>> UpdateCustomer(CustomerUserUpdateRequestDto request, int requestMaker);
        Task<ApiResponse<string>> DeleteCustomer(int userId, int requestMaker);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<List<CustomerUserResponseDto>>> GetAllCustomers(PaginationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<CustomerUserResponseDto>> GetCustomerById(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<CustomerUserResponseDto>> CreateCustomer(CustomerUserRegistrationRequestDto request, string userRoleName, int requestMaker)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> UpdateCustomer(CustomerUserUpdateRequestDto request, int requestMaker)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> DeleteCustomer(int userId, int requestMaker)
        {
            throw new NotImplementedException();
        }

        #region Helper methods for customer user

        #endregion
    }
}
