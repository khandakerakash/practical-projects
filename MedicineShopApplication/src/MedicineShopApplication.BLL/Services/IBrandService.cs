using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Brand;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IBrandService
    {
        Task<ApiResponse<CreateBrandResponseDto>> CreateBrand(CreateBrandRequestDto request, int userId);
        Task<ApiResponse<List<CreateBrandResponseDto>>> CreateBrands(List<CreateBrandRequestDto> requests, int userId);
        Task<ApiResponse<string>> UpdateBrand(UpdateBrandRequestDto request, int brandId, int userId);
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

        public async Task<ApiResponse<CreateBrandResponseDto>> CreateBrand(CreateBrandRequestDto request, int userId)
        {
            var validator = new CreateBrandRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CreateBrandResponseDto>(validationResult.Errors);
            }

            if (await BrandExistsByNameAsync(request.Name))
            {
                return new ApiResponse<CreateBrandResponseDto>(null, false, "A brand with this name already exists.");
            }

            var normalizedBrandName = GeneralUtils.NormalizeName(request.Name);

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
                    CreatedBy = userId
                };

                newBrands.Add(newBrand);

                createdBrandDtos.Add(new CreateBrandResponseDto
                {
                    Code = newBrand.Code,
                    Name = newBrand.Name,
                    NormalizedName = newBrand.NormalizedName,
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

            if (brands.Any(x => x.BrandId != brandId))
            {
                return new ApiResponse<string>(null, false, "A brand with this name already exists.");
            }

            updatingBrand.Code = request.Code;
            updatingBrand.Name = request.Name;
            updatingBrand.NormalizedName = normalizedBrandName;
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

            if (brands == null || !brands.Any())
            {
                return new ApiResponse<List<DropdownOptionDto>>(null, false, "No brands were found.");
            }

            return new ApiResponse<List<DropdownOptionDto>>(brands, true, "Brand dropdown options retrieved successfully.");
        }
        
        #region Brand Helper Methods START
        /// <summary>
        /// Checks if a brand with the specified name already exists in the repository by normalizing the name for consistent comparison.
        /// </summary>
        /// <param name="name">The brand name to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the brand exists; otherwise, false.</returns>
        private async Task<bool> BrandExistsByNameAsync(string name)
        {
            var normalizedBrandName = GeneralUtils.NormalizeName(name);

            return await _unitOfWork.BrandRepository
                .FindByConditionWithTrackingAsync(x => x.NormalizedName == normalizedBrandName)
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
