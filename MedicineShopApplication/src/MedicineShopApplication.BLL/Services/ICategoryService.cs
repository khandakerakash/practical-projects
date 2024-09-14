using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.DLL.UOW;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task<CategoryDto> AddCategory(CategoryInsertDto categoryInsertDto);
        void UpgradeCategory();
    }

    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<CategoryDto> AddCategory(CategoryInsertDto categoryInsertDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryDto>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpgradeCategory()
        {
            throw new NotImplementedException();
        }
    }
}
