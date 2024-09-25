using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.DLL.Models.General;
using System.Security.Claims;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task<ApiResponse<CreateCategoryResponseDto>> CreateCategory(CreateCategoryRequestDto request, int userId);
        Task UpdateCategory(UpdateCategoryRequestDto request);
        Task DeleteCategory(int id);
        Task<ApiResponse<List<DropdownOptionDto>>>  GetCategoryDropdownOptions();
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

        public async Task<ApiResponse<CreateCategoryResponseDto>> CreateCategory(CreateCategoryRequestDto request, int userId)
        {
            var validator = new CreateCategoryRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CreateCategoryResponseDto>(validationResult.Errors);
            }

            if(await CategoryExistsByNameAsync(request.Name))
            {
                return new ApiResponse<CreateCategoryResponseDto>(null, false, "A category with this name already exists.");
            }

            var generatedCode = await GenerateCategoryCodeAsync(request.Name);

            var newCategory = new Category()
            {
                Code = generatedCode,
                Name = request.Name,
                Description = request.Description,
                CreatedBy = userId
            };

            await _unitOfWork.CategoryRepository.CreateAsync(newCategory);

            if(!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<CreateCategoryResponseDto>(null, false, "An error occurred while creating the category.");
            }

            var createdCategoryDto = new CreateCategoryResponseDto()
            {
                CategoryDtoId = newCategory.CategoryId,
                Name = newCategory.Name,
                Code = newCategory.Code,
                Description = newCategory.Description
            };

            return new ApiResponse<CreateCategoryResponseDto>(createdCategoryDto, true, "Category created successfully.");
        }

        #region Helper methods for Create Category

        /// <summary>
        /// Checks if a category with the given name already exists.
        /// </summary>
        /// <param name="categoryName">The creation request containing the category's name.</param>
        /// <returns>Returns true if the category already exists, otherwise false.</returns>
        private async Task<bool> CategoryExistsByNameAsync(string categoryName)
        {
            return await _unitOfWork.CategoryRepository
                .FindByConditionAsync(x => x.Name.ToLower() == categoryName.ToLower())
                .AnyAsync();
        }

        /// <summary>
        /// Generates a unique category code based on the given category name.
        /// This method dynamically creates a prefix using the first few letters of the category name:
        /// - For single-word names, it uses the first 3 letters of the word.
        /// - For multi-word names, it uses the first letter of each word, up to a maximum of 3 words.
        /// The method then checks the database to find how many categories already exist with the same prefix,
        /// and appends the next available numeric suffix (starting from 0001) to ensure the generated code is unique.
        /// </summary>
        /// <param name="categoryName">The name of the category for which the code will be generated.</param>
        /// <returns>A unique category code in the format "PREFIX-####", where PREFIX is derived from the category name.</returns>
        private async Task<string> GenerateCategoryCodeAsync(string categoryName)
        {
            // Remove special characters like parentheses, dashes, underscores
            string cleanedName = new string(categoryName.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

            string[] words = cleanedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string prefix;

            if (words.Length == 1)
            {
                // For single-word names, take the first 3 letters (or fewer if the word is short)
                prefix = words[0].Substring(0, Math.Min(3, words[0].Length)).ToUpper();
            }
            else
            {
                // For multi-word names, take the first letter of each word, up to 3 words
                prefix = string.Join("", words.Take(3).Select(word => word.Substring(0, 1).ToUpper()));
            }

            // Ensure the prefix is clean and no special characters are included
            return await GenerateUniqueCodeAsync(prefix);
        }

        /// <summary>
        /// Generates a unique category code by appending an incremented numeric suffix to the provided prefix.
        /// This method checks the database for existing codes with the same prefix and increments the suffix
        /// until a unique code is generated. The final code will be in the format "PREFIX-####".
        /// </summary>
        /// <param name="prefix">The prefix derived from the category name.</param>
        /// <returns>A unique category code in the format "PREFIX-####".</returns>
        private async Task<string> GenerateUniqueCodeAsync(string prefix)
        {
            int count = await _unitOfWork.CategoryRepository.GetCategoryCountByPrefixAsync(prefix);

            string generatedCode;
            int suffix = count + 1;
            bool isUnique;

            do
            {
                generatedCode = $"{prefix}-{suffix.ToString("D4")}"; // Format the suffix with leading zeroes (0001, 0002, etc.)
                isUnique = !(await _unitOfWork.CategoryRepository
                    .FindByConditionAsync(x => x.Code == generatedCode)
                    .AnyAsync());

                if (!isUnique)
                {
                    suffix++;
                }
            } while (!isUnique);

            return generatedCode;
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

        public async Task<ApiResponse<List<DropdownOptionDto>>> GetCategoryDropdownOptions()
        {
            var categories = await _unitOfWork.CategoryRepository
                .FindAllAsync()
                .Select(c => new DropdownOptionDto
                {
                    Id = c.CategoryId,
                    IdString = c.Code,
                    Value = c.Name,
                }).ToListAsync();

            if(categories == null || !categories.Any())
            {
                return new ApiResponse<List<DropdownOptionDto>>(null, false, "No categories were found.");
            }

            return new ApiResponse<List<DropdownOptionDto>>(categories, true, "Category dropdown options retrieved successfully.");
        }
    }
}
