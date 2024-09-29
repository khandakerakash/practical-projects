using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Category;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace MedicineShopApplication.BLL.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task<ApiResponse<CreateCategoryResponseDto>> CreateCategory(CreateCategoryRequestDto request, int userId);
        Task<ApiResponse<List<CreateCategoryResponseDto>>> CreateCategories(List<CreateCategoryRequestDto> requests, int userId);
        Task UpdateCategory(UpdateCategoryRequestDto request);
        Task DeleteCategory(int id);
        Task<ApiResponse<List<DropdownOptionDto>>>  GetCategoryDropdownOptions();
    }

    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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

            var generatedCode = await GenerateUniqueCategoryCodeAsync(request.Name);

            if (generatedCode == null)
            {
                return new ApiResponse<CreateCategoryResponseDto>(null, false, $"Generated code is null for category name: {request.Name}");
            }

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

            var user = await _userManager.FindByIdAsync(userId.ToString());
            var createdByName = user != null
                ? $"{(user.Title ?? "")} {(user.FirstName ?? "")} {(user.LastName ?? "")}".Trim()
                : "Unknown User";

            var createdCategoryDto = new CreateCategoryResponseDto()
            {
                Name = newCategory.Name,
                Code = newCategory.Code,
                Description = newCategory.Description,
                createdByName = createdByName
            };

            return new ApiResponse<CreateCategoryResponseDto>(createdCategoryDto, true, "Category created successfully.");
        }

        public async Task<ApiResponse<List<CreateCategoryResponseDto>>> CreateCategories(List<CreateCategoryRequestDto> requests, int userId)
        {
            var validator = new CreateCategoryRequestDtoValidator();

            foreach (var request in requests)
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return new ApiResponse<List<CreateCategoryResponseDto>>(validationResult.Errors);
                }
            }

            var duplicateCheckResult = await CheckForDuplicateCategoriesAsync(requests);
            if (!duplicateCheckResult.IsSuccess)
            {
                return duplicateCheckResult;
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            var createdByName = user != null
                ? $"{(user.Title ?? "")} {(user.FirstName ?? "")} {(user.LastName ?? "")}".Trim()
                : "Unknown User";


            var newCategories = new List<Category>();
            var responseDtos = new List<CreateCategoryResponseDto>();
            var requestCodeMap = await GenerateUniqueCategoryCodesForBulkAsync(requests);

            foreach (var request in requests)
            {
                var generatedCode = requestCodeMap[request.Name];

                if (generatedCode == null)
                {
                    return new ApiResponse<List<CreateCategoryResponseDto>>(null, false, $"Generated code is null for category name: {request.Name}");
                }

                var newCategory = new Category
                {
                    Code = generatedCode,
                    Name = request.Name,
                    Description = request.Description,
                    CreatedBy = userId
                };

                newCategories.Add(newCategory);

                responseDtos.Add(new CreateCategoryResponseDto
                {
                    Code = newCategory.Code,
                    Name = newCategory.Name,
                    Description = newCategory.Description,
                    createdByName = createdByName
                });
            }

            await _unitOfWork.CategoryRepository.CreateRangeAsync(newCategories);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<List<CreateCategoryResponseDto>>(null, false, "An error occurred while creating the categories.");
            }

            return new ApiResponse<List<CreateCategoryResponseDto>>(responseDtos, true, "Categories created successfully.");
        }

        #region Helper Methods for Create Category

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
        /// Checks for duplicate categories in the given list of category requests by comparing with existing categories in the database.
        /// It fetches the existing categories with names matching any of the request names, then compares them in a case-insensitive manner.
        /// If any duplicates are found, the method returns an error response; otherwise, it returns a success response.
        /// </summary>
        /// <param name="requests">A list of category creation requests (CreateCategoryRequestDto) containing the category details.</param>
        /// <returns>An ApiResponse containing a success message if no duplicates are found, or an error message if duplicates exist.</returns>
        private async Task<ApiResponse<List<CreateCategoryResponseDto>>> CheckForDuplicateCategoriesAsync(List<CreateCategoryRequestDto> requests)
        {
            var requestNames = requests.Select(request => request.Name.ToLower()).ToHashSet();

            var existingCategories = await _unitOfWork.CategoryRepository
                .FindByConditionAsync(x => requestNames.Contains(x.Name.ToLower()))
                .ToListAsync();

            if (existingCategories.Any())
            {
                var duplicateNames = existingCategories.Select(b => b.Name).ToList();
                return new ApiResponse<List<CreateCategoryResponseDto>>(
                    null,
                    false,
                    $"Duplicate brands found: {string.Join(", ", duplicateNames)}"
                );
            }

            return new ApiResponse<List<CreateCategoryResponseDto>>(null, true, "No duplicate category found.");
        }

        /// <summary>
        /// Generates a unique category code based on the category name.
        /// The code consists of a prefix derived from the first three alphanumeric characters of the category name,
        /// followed by a unique numeric suffix that is determined by the highest existing code with the same prefix in the database.
        /// The numeric suffix is incremented to ensure uniqueness for new categories.
        /// 
        /// The resulting code is formatted as "PREFIX-###", where PREFIX is derived from the category name,
        /// and ### is a zero-padded, incremented numeric suffix (e.g., "HA-001").
        /// </summary>
        /// <param name="categoryName">The name of the category for which the code is being generated.</param>
        /// <returns>A unique category code in the format "PREFIX-###".</returns>
        public async Task<string> GenerateUniqueCategoryCodeAsync(string categoryName)
        {
            string prefix = GeneratePrefixFromCategoryName(categoryName);

            var latestCategory = await _unitOfWork.CategoryRepository
                .FindByConditionAsync(c => c.Code.StartsWith(prefix))
                .OrderByDescending(c => c.Code)
                .FirstOrDefaultAsync();

            int nextSuffix = 1;

            if (latestCategory != null)
            {
                string suffixString = latestCategory.Code.Substring(prefix.Length + 1);
                if (int.TryParse(suffixString, out int suffix))
                {
                    nextSuffix = suffix + 1;
                }
            }

            string generatedCode = $"{prefix}-{nextSuffix.ToString("D3")}";

            return generatedCode;
        }

        /// <summary>
        /// Generates unique category codes for a list of category names in bulk.
        /// Each category code is created by calling the GenerateUniqueCategoryCodeAsync method, 
        /// which generates a prefix based on the first three alphanumeric characters of the category name 
        /// and appends a unique numeric suffix to ensure global uniqueness across all category codes.
        /// 
        /// The resulting codes are returned as a list of strings in the format "PREFIX-###".
        /// </summary>
        /// <param name="categoryNames">A list of category names for which the unique codes are being generated.</param>
        /// <returns>A list of unique category codes, one for each category name, formatted as "PREFIX-###".</returns>

        public async Task<Dictionary<string, string>> GenerateUniqueCategoryCodesForBulkAsync(List<CreateCategoryRequestDto> requests)
        {
            var requestCodeMap = new Dictionary<string, string>();

            var prefixGroups = new Dictionary<string, int>();

            foreach (var request in requests)
            {
                string prefix = GeneratePrefixFromCategoryName(request.Name);

                if (!prefixGroups.ContainsKey(prefix))
                {
                    var latestCategory = await _unitOfWork.CategoryRepository
                        .FindByConditionAsync(c => c.Code.StartsWith(prefix))
                        .OrderByDescending(c => c.Code)
                        .FirstOrDefaultAsync();

                    int latestSuffix = 1;

                    if (latestCategory != null)
                    {
                        string suffixString = latestCategory.Code.Substring(prefix.Length + 1);
                        if (int.TryParse(suffixString, out int suffix))
                        {
                            latestSuffix = suffix + 1;
                        }
                    }

                    prefixGroups[prefix] = latestSuffix;
                }

                int currentSuffix = prefixGroups[prefix];
                string generatedCode = $"{prefix}-{currentSuffix.ToString("D3")}";

                prefixGroups[prefix]++;

                requestCodeMap[request.Name] = generatedCode;
            }

            return requestCodeMap;
        }

        /// <summary>
        /// Generates a prefix for a category code based on the given category name.
        /// 
        /// The prefix is generated as follows:
        /// - If the first word contains an alphanumeric character combination (e.g., "H2"), it uses that and appends the first letter of the second word (if available).
        /// - For single-word names, it uses the first three letters of the word.
        /// - For two-word names, it combines the first letter of each word.
        /// - For three or more words, it combines the first letter of the first three words.
        /// 
        /// Special characters are removed, and the result is returned in uppercase.
        /// </summary>
        /// <param name="categoryName">The category name from which to generate the prefix.</param>
        /// <returns>A prefix string based on the category name.</returns>

        private string GeneratePrefixFromCategoryName(string categoryName)
        {
            string cleanedName = new string(categoryName.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
            string[] words = cleanedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string prefix = string.Empty;


            if (words.Length > 0 && words[0].Length >= 2 && char.IsLetter(words[0][0]) && char.IsDigit(words[0][1]))
            {
                prefix = words[0].Substring(0, 2).ToUpper();

                if (words.Length > 1)
                {
                    prefix += words[1].Substring(0, 1).ToUpper();
                }
                if (words.Length > 2)
                {
                    prefix += words[2].Substring(0, 1).ToUpper();
                }
            }
            else
            {
                if (words.Length == 1)
                {
                    prefix = words[0].Substring(0, Math.Min(3, words[0].Length)).ToUpper();
                }
                else if (words.Length == 2)
                {
                    prefix = $"{words[0].Substring(0, 1).ToUpper()}{words[1].Substring(0, 1).ToUpper()}";
                }
                else if (words.Length >= 3)
                {
                    prefix = $"{words[0].Substring(0, 1).ToUpper()}{words[1].Substring(0, 1).ToUpper()}{words[2].Substring(0, 1).ToUpper()}";
                }
            }

            return prefix;
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
