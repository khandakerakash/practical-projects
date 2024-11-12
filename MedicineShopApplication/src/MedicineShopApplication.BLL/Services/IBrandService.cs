using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Brand;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.Extension;

namespace MedicineShopApplication.BLL.Services
{
    public interface IBrandService
    {
        Task<ApiResponse<List<BrandResponseDto>>> GetAllBrands(PaginationRequest request);
        Task<ApiResponse<BrandResponseDto>> GetBrandById(int brandId);
        Task<ApiResponse<CreateBrandResponseDto>> CreateBrand(CreateBrandRequestDto request, int userId);
        Task<ApiResponse<List<CreateBrandResponseDto>>> CreateBrands(List<CreateBrandRequestDto> requests, int userId);
        Task<ApiResponse<string>> UpdateBrand(UpdateBrandRequestDto request, int brandId, int userId);
        Task<ApiResponse<string>> DeleteBrand(int brandId, int userId);
        Task<ApiResponse<string>> UndoDeletedBrand(int brandId, int userId);
        Task<ApiResponse<List<DropdownOptionDto>>> GetBrandDropdownOptions();
    }

    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public BrandService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ApiResponse<List<BrandResponseDto>>> GetAllBrands(PaginationRequest request)
        {
            var skipValue = PaginationUtils.SkipValue(request.Page, request.PageSize);

            var brandsQuery = _unitOfWork.BrandRepository.FindAllAsync();

            brandsQuery = SortBrandsQuery(brandsQuery, request.SortBy);

            var totalBrandsCount = await brandsQuery.CountAsync();

            var userIds = brandsQuery.Select(x => x.CreatedBy)
                .Union(brandsQuery.Where(x => x.UpdatedBy.HasValue).Select(x => x.UpdatedBy.Value))
                .Distinct();

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var brandsList = await brandsQuery
                .Skip(skipValue)
                .Take(request.PageSize)
                .Select(x => new BrandResponseDto
                {
                    BrandId = x.BrandId,
                    Code = x.Code,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    Description = x.Description,

                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = users.ContainsKey(x.CreatedBy)
                                    ? users[x.CreatedBy].GetFullName()
                                    : "",

                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.UpdatedBy.HasValue && users.ContainsKey(x.UpdatedBy.Value)
                                    ? users[x.UpdatedBy.Value].GetFullName()
                                    : ""

                })
                .ToListAsync();

            return new ApiPaginationResponse<List<BrandResponseDto>>(brandsList, request.Page, request.PageSize, totalBrandsCount);
        }

        public async Task<ApiResponse<BrandResponseDto>> GetBrandById(int brandId)
        {
            var brandQuery = _unitOfWork.BrandRepository
                .FindByConditionAsync(x => x.BrandId == brandId);

            var brand = await brandQuery.FirstOrDefaultAsync();
            if (brand.HasNoValue())
            {
                return new ApiResponse<BrandResponseDto>(null, false, "Brand not found.");
            }

            var userIds = new List<int> { brand.CreatedBy };
            if (brand.UpdatedBy.HasValue)
            {
                userIds.Add(brand.UpdatedBy.Value);
            }

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var brandResponse = await brandQuery
                .Select(x => new BrandResponseDto
                {
                    BrandId = x.BrandId,
                    Code = x.Code,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    Description = x.Description,

                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = users.ContainsKey(x.CreatedBy)
                                    ? users[x.CreatedBy].GetFullName()
                                    : "",

                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedByName = x.UpdatedBy.HasValue && users.ContainsKey(x.UpdatedBy.Value)
                                    ? users[x.UpdatedBy.Value].GetFullName()
                                    : ""
                })
                .FirstOrDefaultAsync();

            return new ApiResponse<BrandResponseDto>(brandResponse, true, "Brand found.");
        }

        public async Task<ApiResponse<CreateBrandResponseDto>> CreateBrand(CreateBrandRequestDto request, int userId)
        {
            var validator = new CreateBrandRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CreateBrandResponseDto>(validationResult.Errors);
            }

            var normalizedBrandName = GeneralUtils.NormalizeName(request.Name);
            if (await BrandExistsByNameAsync(normalizedBrandName))
            {
                return new ApiResponse<CreateBrandResponseDto>(null, false, "A brand with this name already exists.");
            }

            var brand = new Brand
            {
                Code = request.Code,
                Name = request.Name,
                NormalizedName = normalizedBrandName,
                CreatedBy = userId
            };

            await _unitOfWork.BrandRepository.CreateAsync(brand);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<CreateBrandResponseDto>(null, false, "An error occurred while creating the brand.");
            }

            var createdByName = await _userManager.GetFullNameByIdAsync(userId);

            var createdBrandDto = new CreateBrandResponseDto()
            {
                Code = brand.Code,
                Name = brand.Name,
                NormalizedName = brand.NormalizedName,
                CreatedByName = createdByName
            };

            return new ApiResponse<CreateBrandResponseDto>(createdBrandDto, true, "Brand created successfully.");
        }

        public async Task<ApiResponse<List<CreateBrandResponseDto>>> CreateBrands(List<CreateBrandRequestDto> requests, int userId)
        {
            var validator = new CreateBrandRequestDtoValidator();

            foreach (var request in requests)
            {
                var validationResult = await validator.ValidateAsync(request);
                if(!validationResult.IsValid)
                {
                    return new ApiResponse<List<CreateBrandResponseDto>>(validationResult.Errors);
                }
            }

            var duplicateCheckResult = await CheckForDuplicateBrandsAsync(requests);
            if (!duplicateCheckResult.IsSuccess)
            {
                return duplicateCheckResult;
            }

            var createdByName = await _userManager.GetFullNameByIdAsync(userId);

            var newBrands = new List<Brand>();
            var createdBrandDtos = new List<CreateBrandResponseDto>();

            foreach (var request in requests)
            {
                var newBrand = new Brand
                {
                    Code = request.Code,
                    Name = request.Name,
                    NormalizedName = GeneralUtils.NormalizeName(request.Name),
                    Description = request.Description,
                    CreatedBy = userId
                };

                newBrands.Add(newBrand);

                createdBrandDtos.Add(new CreateBrandResponseDto
                {
                    Code = newBrand.Code,
                    Name = newBrand.Name,
                    NormalizedName = newBrand.NormalizedName,
                    Description = newBrand.Description,
                    CreatedByName = createdByName
                });
            }

            await _unitOfWork.BrandRepository.CreateRangeAsync(newBrands);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<List<CreateBrandResponseDto>>(null, false, "An error occurred while creating the brand.");
            }

            return new ApiResponse<List<CreateBrandResponseDto>>(createdBrandDtos, true, "Brand created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateBrand(UpdateBrandRequestDto request, int brandId, int userId)
        {
            var validator = new UpdateBrandRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var normalizedBrandName = GeneralUtils.NormalizeName(request.Name);
            var brands = await _unitOfWork.BrandRepository
                .FindByConditionWithTrackingAsync(x => x.BrandId == brandId || x.NormalizedName == normalizedBrandName)
                .ToListAsync();

            var updatingBrand = brands.FirstOrDefault(x => x.BrandId == brandId);

            if (updatingBrand.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Brand not found.");
            }

            var isNameExisting = brands.Any(x => x.BrandId != brandId && x.NormalizedName == normalizedBrandName);

            if (isNameExisting)
            {
                return new ApiResponse<string>(null, false, "A brand with this name already exists.");
            }

            updatingBrand.Code = request.Code;
            updatingBrand.Name = request.Name;
            updatingBrand.NormalizedName = normalizedBrandName;
            updatingBrand.Description = request.Description;
            updatingBrand.UpdatedBy = userId;
            updatingBrand.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.BrandRepository.Update(updatingBrand);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the brand.");
            }

            return new ApiResponse<string>(null, true, "Brand updated successfully.");
        }

        public async Task<ApiResponse<List<DropdownOptionDto>>> GetBrandDropdownOptions()
        {
            var brands = await _unitOfWork.BrandRepository
                .FindAllAsync()
                .Select(x => new DropdownOptionDto
                {
                    Id = x.BrandId,
                    IdString = x.Code,
                    Value = x.Name,
                }).ToListAsync();

            if (!brands.Any())
            {
                return new ApiResponse<List<DropdownOptionDto>>(null, true, "No brands were found.");
            }

            return new ApiResponse<List<DropdownOptionDto>>(brands, true, "Brand dropdown options retrieved successfully.");
        }

        public async Task<ApiResponse<string>> DeleteBrand(int brandId, int userId)
        {
            var brand = await _unitOfWork.BrandRepository
                .FindByConditionWithTrackingAsync(x => x.BrandId == brandId)
                .FirstOrDefaultAsync();

            if (brand.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Brand not found.");
            }

            brand.UpdatedBy = userId;
            brand.UpdatedAt = DateTime.Now;
            _unitOfWork.BrandRepository.SoftDelete(brand);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while deleting the brand.");
            }

            return new ApiResponse<string>(null, true, "Brand deleted successfully.");
        }

        public async Task<ApiResponse<string>> UndoDeletedBrand(int brandId, int userId)
        {
            var brand = await _unitOfWork.BrandRepository
                .FindByConditionWithTrackingAsync(x => x.BrandId == brandId && x.IsDeleted, includeDeleted: true)
                .FirstOrDefaultAsync();

            if (brand.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Brand not found.");
            }

            brand.UpdatedBy = userId;
            brand.UpdatedAt = DateTime.Now;
            _unitOfWork.BrandRepository.UndoSoftDelete(brand);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while undoing the brand.");
            }

            return new ApiResponse<string>(null, true, "Brand undoed successfully.");
        }

        #region Brand Helper Methods START
        /// <summary>
        /// Sorts the provided <see cref="IQueryable{Brand}"/> based on the specified sorting criteria.
        /// The sorting criteria are provided as a string in the format "property direction", 
        /// where "direction" can be "asc" for ascending or "desc" for descending. 
        /// Multiple criteria can be specified, separated by commas, e.g., "name desc, code asc".
        /// If no valid criteria are specified, the default sorting is by BrandId in descending order.
        /// </summary>
        /// <param name="brandsQuery">The queryable collection of <see cref="Brand"/> to be sorted.</param>
        /// <param name="sortBy">A string containing sorting criteria, e.g., "name desc, code asc".</param>
        /// <returns>An <see cref="IQueryable{Brand}"/> sorted based on the specified criteria.</returns>
        private IQueryable<Brand> SortBrandsQuery(IQueryable<Brand> brandsQuery, string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                // Default sorting
                return brandsQuery.OrderByDescending(x => x.BrandId);
            }

            // Split multiple sort criteria (e.g., "name desc, code asc")
            var sortParts = sortBy.Split(',');

            foreach (var sortPart in sortParts)
            {
                var trimmedPart = sortPart.Trim();
                var orderDescending = trimmedPart.EndsWith(" desc", StringComparison.OrdinalIgnoreCase);
                var propertyName = orderDescending
                    ? trimmedPart[0..^5]
                    : trimmedPart.EndsWith(" asc", StringComparison.OrdinalIgnoreCase)
                        ? trimmedPart[0..^4]
                        : trimmedPart;


                if (propertyName.ToLower() == "id")
                {
                    brandsQuery = orderDescending
                        ? brandsQuery.OrderByDescending(x => x.BrandId)
                        : brandsQuery.OrderBy(x => x.BrandId);
                }

                if (propertyName.ToLower() == "code")
                {
                    brandsQuery = orderDescending
                        ? brandsQuery.OrderByDescending(x => x.Code)
                        : brandsQuery.OrderBy(x => x.Code);
                }

                if (propertyName.ToLower() == "name")
                {
                    brandsQuery = orderDescending
                        ? brandsQuery.OrderByDescending(x => x.Name)
                        : brandsQuery.OrderBy(x => x.Name);
                }
            }

            return brandsQuery; ;
        }

        /// <summary>
        /// Checks if a brand with the specified name already exists in the repository by normalizing the name for consistent comparison.
        /// </summary>
        /// <param name="name">The brand name to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the brand exists; otherwise, false.</returns>
        private async Task<bool> BrandExistsByNameAsync(string name)
        {
            return await _unitOfWork.BrandRepository
                .FindByConditionWithTrackingAsync(x => x.NormalizedName == name)
                .AnyAsync();
        }

        /// <summary>
        /// Checks for duplicate brands in the given list of brand requests by comparing with existing brands in the database.
        /// It fetches the existing brands with names matching any of the request names, then compares them in a normalized manner.
        /// If any duplicates are found, the method returns an error response with details; otherwise, it returns a success response.
        /// </summary>
        /// <param name="requests">A list of brand creation requests (CreateBrandRequestDto) containing the brand details.</param>
        /// <returns>An ApiResponse containing a success message if no duplicates are found, or an error message if duplicates exist.</returns>
        private async Task<ApiResponse<List<CreateBrandResponseDto>>> CheckForDuplicateBrandsAsync(List<CreateBrandRequestDto> requests)
        {
            var requestNames = requests.Select(request => GeneralUtils.NormalizeName(request.Name)).ToHashSet();

            var existingBrands = await _unitOfWork.BrandRepository
                .FindByConditionWithTrackingAsync(x => requestNames.Contains(x.NormalizedName))
                .ToListAsync();

            if (existingBrands.Any())
            {
                var duplicateNames = existingBrands.Select(b => b.Name).ToList();
                return new ApiResponse<List<CreateBrandResponseDto>>(
                    null,
                    false,
                    $"Duplicate brands found: {string.Join(", ", duplicateNames)}"
                );
            }

            return new ApiResponse<List<CreateBrandResponseDto>>(null, true, "No duplicate brand found.");
        }
        #endregion
    }
}
