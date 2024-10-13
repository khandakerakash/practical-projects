using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Dtos.Inventory;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.Enums;
using MedicineShopApplication.DLL.Models.General;
using MedicineShopApplication.DLL.UOW;
using Microsoft.EntityFrameworkCore;

namespace MedicineShopApplication.BLL.Services
{
    public interface IInventoryService
    {
        Task<ApiResponse<List<InventoryResponseDto>>> GetAllInventories(PaginationRequest request);
        Task<ApiResponse<InventoryResponseDto>> GetInventoryById(int inventoryId);
        Task<ApiResponse<string>> CreateInventory(int requestMaker, CreateInventoryRequestDto request);
        Task<ApiResponse<string>> UpdateInventory(int requestMaker, int inventoryId, UpdateInventoryRequestDto request);
    }

    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<List<InventoryResponseDto>>> GetAllInventories(PaginationRequest request)
        {
            var skipValue = PaginationUtils.SkipValue(request.Page, request.PageSize);

            var inventoriesQuery = _unitOfWork.InventoryRepository
                .FindAllAsync();

            inventoriesQuery = SortInventoriesQuery(inventoriesQuery, request.SortBy);

            var totalInventoriesQuery = await inventoriesQuery.CountAsync();

            var userIds = inventoriesQuery.Select(x => x.CreatedBy)
                .Union(inventoriesQuery.Where(x => x.UpdatedBy.HasValue).Select(x => x.UpdatedBy.Value))
                .Distinct();

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var inventoryList = await inventoriesQuery
                .Include(x => x.Product)
                .Include(x => x.UnitOfMeasure)
                .Skip(skipValue)
                .Take(request.PageSize)
                .Select(x => new InventoryResponseDto
                {
                    InventoryId = x.InventoryId,
                    QuantityInStock = x.QuantityInStock,
                    ReorderLevel = x.ReorderLevel,
                    Status = InventoryStatusUtils.GetInventoryStatusDisplayName(x.Status),
                    Location = x.Location,
                    Notes = x.Notes,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    UnitOfMeasureId = x.UnitOfMeasureId,
                    UnitOfMeasureName = x.UnitOfMeasure.Name
                }).ToListAsync();

            return new ApiResponse<List<InventoryResponseDto>>(inventoryList, true, "Inventories retrieved successfully.");
        }

        public async Task<ApiResponse<InventoryResponseDto>> GetInventoryById(int inventoryId)
        {
            var inventory = await _unitOfWork.InventoryRepository
                .FindByConditionAsync(x => x.InventoryId == inventoryId)
                .Include(x => x.Product)
                .Include (x => x.UnitOfMeasure)
                .FirstOrDefaultAsync();

            if (inventory.HasNoValue())
            {
                return new ApiResponse<InventoryResponseDto>(null, false, "Inventory not found.");
            }

            var inventoryResponse = new InventoryResponseDto
            {
                InventoryId = inventory.InventoryId,
                QuantityInStock = inventory.QuantityInStock,
                ReorderLevel = inventory.ReorderLevel,
                Status = InventoryStatusUtils.GetInventoryStatusDisplayName(inventory.Status),
                Location = inventory.Location,
                Notes = inventory.Notes,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product.Name,
                UnitOfMeasureId = inventory.UnitOfMeasureId,
                UnitOfMeasureName = inventory.UnitOfMeasure.Name
            };

            return new ApiResponse<InventoryResponseDto>(inventoryResponse, true, "Inventory retrieved successfully.");
        }

        public async Task<ApiResponse<string>> CreateInventory(int requestMaker, CreateInventoryRequestDto request)
        {
            var validator = new CreateInventoryRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }
            
            var product = await _unitOfWork.ProductRepository
                .FindByConditionWithTrackingAsync(x => x.ProductId == request.ProductId)
                .FirstOrDefaultAsync();

            if (product.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Product not found.");
            }

            var unitOfMeasure = await _unitOfWork.UnitOfMeasureRepository
                .FindByConditionWithTrackingAsync(x => x.UnitOfMeasureId == request.UnitOfMeasureId)
                .FirstOrDefaultAsync();

            if (!unitOfMeasure.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Unit of measure not found.");
            }

            var inventory = new Inventory
            {
                ProductId = request.ProductId,
                QuantityInStock = request.QuantityInStock,
                ReorderLevel = request.ReorderLevel,
                Location = request.Location,
                Notes = request.Notes,
                UnitOfMeasureId = request.UnitOfMeasureId,
                Status = InventoryStatus.Available, // default status when create an inventory

                CreatedBy = requestMaker
            };

            await _unitOfWork.InventoryRepository.CreateAsync(inventory);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occured while creating the inventory.");
            }

            return new ApiResponse<string>(null, false, "Inventory created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateInventory(int requestMaker, int inventoryId, UpdateInventoryRequestDto request)
        {
            var validator = new UpdateInventoryRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var inventory = await _unitOfWork.InventoryRepository
            .FindByConditionWithTrackingAsync(x => x.InventoryId == inventoryId)
            .FirstOrDefaultAsync();

            if (inventory == null)
            {
                return new ApiResponse<string>(null, false, "Inventory not found.");
            }

            var status = Enum.Parse<InventoryStatus>(request.Status, true);

            inventory.QuantityInStock = request.QuantityInStock;
            inventory.ReorderLevel = request.ReorderLevel;
            inventory.Location = request.Location;
            inventory.Notes = request.Notes;
            inventory.UnitOfMeasureId = request.UnitOfMeasureId;
            inventory.Status = status;

            inventory.UpdatedBy = requestMaker;
            inventory.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.InventoryRepository.Update(inventory);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the inventory.");
            }

            return new ApiResponse<string>(null, true, "Inventory updated successfully.");
        }

        #region Inventory Helper Methods START
        /// <summary>
        /// Sorts the provided <see cref="IQueryable{Inventory}"/> based on the specified sorting criteria.
        /// The sorting criteria are provided as a string in the format "property direction", 
        /// where "direction" can be "asc" for ascending or "desc" for descending. 
        /// Multiple criteria can be specified, separated by commas, e.g., "name desc, code asc".
        /// If no valid criteria are specified, the default sorting is by InventoryId in descending order.
        /// </summary>
        /// <param name="inventoriesQuery">The queryable collection of <see cref="Inventory"/> to be sorted.</param>
        /// <param name="sortBy">A string containing sorting criteria, e.g., "name desc, code asc".</param>
        /// <returns>An <see cref="IQueryable{Inventory}"/> sorted based on the specified criteria.</returns>
        private IQueryable<Inventory> SortInventoriesQuery(IQueryable<Inventory> inventoriesQuery, string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                // Default sorting
                return inventoriesQuery.OrderByDescending(x => x.InventoryId);
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
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.InventoryId)
                        : inventoriesQuery.OrderBy(x => x.InventoryId);
                }

                if (propertyName.ToLower() == "stock")
                {
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.QuantityInStock)
                        : inventoriesQuery.OrderBy(x => x.QuantityInStock);
                }

                if (propertyName.ToLower() == "reorder")
                {
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.ReorderLevel)
                        : inventoriesQuery.OrderBy(x => x.ReorderLevel);
                }

                if (propertyName.ToLower() == "status")
                {
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.Status)
                        : inventoriesQuery.OrderBy(x => x.Status);
                }

                if (propertyName.ToLower() == "location")
                {
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.Location)
                        : inventoriesQuery.OrderBy(x => x.Location);
                }

                if (propertyName.ToLower() == "notes")
                {
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.Notes)
                        : inventoriesQuery.OrderBy(x => x.Notes);
                }

                if (propertyName.ToLower() == "productId")
                {
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.ProductId)
                        : inventoriesQuery.OrderBy(x => x.ProductId);
                }

                if (propertyName.ToLower() == "unitofmeasureid")
                {
                    inventoriesQuery = orderDescending
                        ? inventoriesQuery.OrderByDescending(x => x.UnitOfMeasureId)
                        : inventoriesQuery.OrderBy(x => x.UnitOfMeasureId);
                }
            }

            return inventoriesQuery; ;
        }
        #endregion
    }
}
