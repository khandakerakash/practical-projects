using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Dtos.Product;

namespace MedicineShopApplication.BLL.Services
{
    public interface IProductService
    {
        Task<CreateProductResponseDto> CreateProduct(CreateProductRequestDto request);
    }

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<CreateProductResponseDto> CreateProduct(CreateProductRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
