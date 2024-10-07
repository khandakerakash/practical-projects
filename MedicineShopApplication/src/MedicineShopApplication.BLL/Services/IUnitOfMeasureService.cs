using MedicineShopApplication.BLL.Dtos.UnitOfMeasure;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bogus.DataSets;
using MedicineShopApplication.BLL.Dtos.Brand;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.BLL.Dtos.Inventory;


namespace MedicineShopApplication.BLL.Services
{
    public interface IUnitOfMeasureService
    {
        Task<ApiResponse<List<UnitOfMeasureResponseDto>>> GetAllUnitOfMeasure();
        Task<ApiResponse<UnitOfMeasureResponseDto>> GetUnitOfMeasureById(int unitOfMeasureId);
        Task<ApiResponse<CreateUnitOfMeasureResponseDto>> CreateUnitOfMeasure(CreateUnitOfMeasureRequestDto request, int userId);
        Task<ApiResponse<string>> UpdateUnitOfMeasure(UpdateUnitOfMeasureRequestDto request, int unitOfMeasureId, int userId);
        Task<ApiResponse<string>> DeleteUnitOfMeasure(int unitOfMeasureId, int userId);
        Task<ApiResponse<List<DropdownOptionDto>>> GetUnitOfMeasureDropdownOptions();
    }

    public class UnitOfMeasureService : IUnitOfMeasureService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnitOfMeasureService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public Task<ApiResponse<List<UnitOfMeasureResponseDto>>> GetAllUnitOfMeasure()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<UnitOfMeasureResponseDto>> GetUnitOfMeasureById(int unitOfMeasureId)
        {
            var unitQuery = _unitOfWork.UnitOfMeasureRepository
                .FindByConditionAsync(x => x.UnitOfMeasureId == unitOfMeasureId);

            var unit = await unitQuery.FirstOrDefaultAsync();
            if (unit.HasNoValue())
            {
                return new ApiResponse<UnitOfMeasureResponseDto>(null, false, "Unit of measure not found.");
            }

            var userIds = new List<int> { unit.CreatedBy };
            if (unit.UpdatedBy.HasValue)
            {
                userIds.Add(unit.UpdatedBy.Value);
            }

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var unitResponse = await unitQuery
                .Select(x => new UnitOfMeasureResponseDto
                {
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    Description = x.Description,

                    ProductDtos = x.Products.Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        Code = p.Code,
                        Name = p.Name,
                        NormalizedName = p.NormalizedName,
                        GenericName = p.GenericName,
                        Description = p.Description,
                        CostPrice = p.CostPrice,
                        SellingPrice = p.SellingPrice,
                        Status = ProductStatusUtils.GetProductStatusDisplayName(p.Status),
                    }).ToList(),

                    InventoryDtos = x.Inventories.Select(i => new InventoryDto
                    {
                        InventoryId = i.InventoryId,
                        QuantityInStock = i.QuantityInStock,
                        ReorderLevel = i.ReorderLevel,
                        Status = i.Status,
                    }).ToList(),

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

            return new ApiResponse<UnitOfMeasureResponseDto>(unitResponse, true, "Unit of measure found.");
        }

        public async Task<ApiResponse<CreateUnitOfMeasureResponseDto>> CreateUnitOfMeasure(CreateUnitOfMeasureRequestDto request, int userId)
        {
            var validator = new CreateUnitOfMeasureRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CreateUnitOfMeasureResponseDto>(validationResult.Errors);
            }

            var normalizedName = GeneralUtils.NormalizeName(request.Name);
            if (await ExistsByNameAsync(normalizedName))
            {
                return new ApiResponse<CreateUnitOfMeasureResponseDto>(null, false, "A Unit of measure with this name already exists.");
            }

            var unitOfMeasure = new UnitOfMeasure
            {
                Name = request.Name,
                NormalizedName = normalizedName,
                Description = request.Description,
                CreatedBy = userId
            };

            await _unitOfWork.UnitOfMeasureRepository.CreateAsync(unitOfMeasure);
            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<CreateUnitOfMeasureResponseDto>(null, false, "An error occurred while creating the Unit of measure.");
            }

            var createdByName = await _userManager.GetFullNameByIdAsync(userId);
            var createdUnitOfMeasure = new CreateUnitOfMeasureResponseDto()
            {
                Name = unitOfMeasure.Name,
                NormalizedName = unitOfMeasure.NormalizedName,
                Description = unitOfMeasure.Description,
                CreatedByName = createdByName
            };

            return new ApiResponse<CreateUnitOfMeasureResponseDto>(createdUnitOfMeasure, true, "Unit of measure created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateUnitOfMeasure(UpdateUnitOfMeasureRequestDto request, int unitOfMeasureId, int userId)
        {
            var validator = new UpdateUnitOfMeasureRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var normalizedName = GeneralUtils.NormalizeName(request.Name);
            var unitOfMeasures = await _unitOfWork.UnitOfMeasureRepository
                .FindByConditionWithTrackingAsync(x => x.UnitOfMeasureId == unitOfMeasureId || x.NormalizedName == normalizedName)
                .ToListAsync();

            var updatingUnitOfMeasure = unitOfMeasures.FirstOrDefault(x => x.UnitOfMeasureId == unitOfMeasureId);
            if (updatingUnitOfMeasure.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Unit of measure not found.");
            }

            var isNameExisting = unitOfMeasures.Any(x => x.UnitOfMeasureId != unitOfMeasureId && x.NormalizedName == normalizedName);

            if (isNameExisting)
            {
                return new ApiResponse<string>(null, false, "A Unit of measure with this name already exists.");
            }

            updatingUnitOfMeasure.Name = request.Name;
            updatingUnitOfMeasure.NormalizedName = normalizedName;
            updatingUnitOfMeasure.Description = request.Description;
            updatingUnitOfMeasure.UpdatedBy = userId;
            updatingUnitOfMeasure.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.UnitOfMeasureRepository.Update(updatingUnitOfMeasure);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the Unit of measure.");
            }

            return new ApiResponse<string>(null, true, "The Unit of measure updated successfully.");
        }

        public Task<ApiResponse<string>> DeleteUnitOfMeasure(int unitOfMeasureId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<DropdownOptionDto>>> GetUnitOfMeasureDropdownOptions()
        {
            var unitOfMeasures = await _unitOfWork.UnitOfMeasureRepository
                .FindAllAsync()
                .Select(x => new DropdownOptionDto
                {
                    Id = x.UnitOfMeasureId,
                    Value = x.Name,
                }).ToListAsync();

            if (!unitOfMeasures.Any())
            {
                return new ApiResponse<List<DropdownOptionDto>>(null, true, "No Unit of measure were found.");
            }

            return new ApiResponse<List<DropdownOptionDto>>(unitOfMeasures, true, "Unit of measure dropdown options retrieved successfully.");
        }

        #region Helper Methods of UnitOfMeasure
        /// <summary>
        /// Checks if a unitOfMeasure with the specified name already exists in the repository by normalizing the name for consistent comparison.
        /// </summary>
        /// <param name="unitOfMeasure">The unitOfMeasure name to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the brand exists; otherwise, false.</returns>
        private async Task<bool> ExistsByNameAsync(string name)
        {
            return await _unitOfWork.UnitOfMeasureRepository
                .FindByConditionWithTrackingAsync(x => x.NormalizedName == name)
                .AnyAsync();
        }

        #endregion
    }
}
