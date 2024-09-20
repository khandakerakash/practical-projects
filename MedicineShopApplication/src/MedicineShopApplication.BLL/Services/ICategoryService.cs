using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.DLL.Models.General;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task<ApiResponse<CategoryDto>> CreateCategory(CreateCategoryRequestDto request);
        Task UpdateCategory(UpdateCategoryRequestDto request);
        Task DeleteCategory(int id);
        Task<List<DropdownOptionDto>>  GetCategoryDropdownOptions();
    }

    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
 

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<CategoryDto>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<CategoryDto>> CreateCategory(CreateCategoryRequestDto request)
        {
            var validator = new CreateCategoryRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CategoryDto>(validationResult.Errors);
            }

            if(await CategoryExistsAsync(request))
            {
                return new ApiResponse<CategoryDto>(null, false, "A category with this code already exists.");
            }

            var newCategory = new Category()
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                CreatedBy = 1
            };

            await _unitOfWork.CategoryRepository.CreateAsync(newCategory);

            if(!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<CategoryDto>(null, false, "An error occurred while creating the category.");
            }

            var createdCategoryDto = new CategoryDto()
            {
                CategoryDtoId = newCategory.CategoryId,
                Name = newCategory.Name,
                Code = newCategory.Code,
                Description = newCategory.Description,
                CreatedBy = newCategory.CreatedBy,
            };

            return new ApiResponse<CategoryDto>(createdCategoryDto, true, "Category created successfully.");
        }

        #region Helper methods for Create Category
        /// <summary>
        /// Checks if a category with the given code already exists.
        /// </summary>
        /// <param name="request">The creation request containing the category's code.</param>
        /// <returns>Returns true if the category already exists, otherwise false.</returns>
        private async Task<bool> CategoryExistsAsync(CreateCategoryRequestDto request)
        {
            return await _unitOfWork.CategoryRepository
                .FindByConditionAsync(x => x.Code == request.Code)
                .AnyAsync();
        }

        #endregion

        public Task UpdateCategory(UpdateCategoryRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DropdownOptionDto>> GetCategoryDropdownOptions()
        {
            throw new NotImplementedException();
        }
    }
}
