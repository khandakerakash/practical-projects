using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Utils;
using MedicineShopApplication.BLL.Extension;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.DLL.Models.Enums;
using MedicineShopApplication.DLL.Models.General;



namespace MedicineShopApplication.BLL.Services
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductResponseDto>>> GetAllProducts(PaginationRequest request);
        Task<ApiResponse<ProductResponseDto>> GetAProduct(int productId);
        Task<ApiResponse<CreateProductResponseDto>> CreateProduct(CreateProductRequestDto request, int userId);
        Task<ApiResponse<string>> UpdateProduct(UpdateProductRequestDto request, int productId, int userId);
        Task<ApiResponse<string>> DeleteProduct(int productId, int userId);
    }

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductService(
            IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ApiResponse<List<ProductResponseDto>>> GetAllProducts(PaginationRequest request)
        {
            var skipValue = PaginationUtils.SkipValue(request.Page, request.PageSize);

            var productsQuery = _unitOfWork.ProductRepository.FindAllAsync();

            productsQuery = SortProductsQuery(productsQuery, request.SortBy);

            var totalProductCount = await productsQuery.CountAsync();

            var userIds = productsQuery.Select(x => x.CreatedBy)
                .Union(productsQuery.Where(x => x.UpdatedBy.HasValue).Select(x => x.UpdatedBy.Value))
                .Distinct();

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var productList = await productsQuery
                .Skip(skipValue)
                .Take(request.PageSize)
                .Select(x => new ProductResponseDto
                {
                    ProductId = x.ProductId,
                    Code = x.Code,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    GenericName = x.GenericName,
                    Description = x.Description,
                    CostPrice = x.CostPrice,
                    SellingPrice = x.SellingPrice,
                    Status = ProductStatusUtils.GetProductStatusDisplayName(x.Status),
                    ImageUrl = x.ImageUrl,
                    Notes = x.Notes,

                    BrandId = x.BrandId,
                    BrandName = x.Brand != null ? x.Brand.Name : "",

                    CategoryId = x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.Name : "",

                    UnitOfMeasureId = x.UnitOfMeasureId,
                    UnitOfMeasureName = x.UnitOfMeasure != null ? x.UnitOfMeasure.Name : "",

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

            return new ApiPaginationResponse<List<ProductResponseDto>>(productList, request.Page, request.PageSize, totalProductCount);
        }

        public async Task<ApiResponse<ProductResponseDto>> GetAProduct(int productId)
        {
            var productQuery = _unitOfWork.ProductRepository
                .FindByConditionAsync(x => x.ProductId == productId);

            var product = await productQuery.FirstOrDefaultAsync();

            if (product.HasNoValue())
            {
                return new ApiResponse<ProductResponseDto>(null, false, "Product not found.");
            }

            var userIds = new List<int> { product.CreatedBy };
            if (product.UpdatedBy.HasValue)
            {
                userIds.Add(product.UpdatedBy.Value);
            }

            var users = await _unitOfWork.UserRepository
                .FindByConditionAsync(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            var productResponse = await productQuery
                .Select(x => new ProductResponseDto
                {
                    ProductId = x.ProductId,
                    Code = x.Code,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    GenericName = x.GenericName,
                    Description = x.Description,
                    CostPrice = x.CostPrice,
                    SellingPrice = x.SellingPrice,
                    Status = ProductStatusUtils.GetProductStatusDisplayName(product.Status),
                    ImageUrl = x.ImageUrl,
                    Notes = x.Notes,

                    BrandId = x.BrandId,
                    BrandName = x.Brand != null ? x.Brand.Name : "",

                    CategoryId = x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.Name : "",

                    UnitOfMeasureId = x.UnitOfMeasureId,
                    UnitOfMeasureName = x.UnitOfMeasure != null ? x.UnitOfMeasure.Name : "",

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

            return new ApiResponse<ProductResponseDto>(productResponse, true, "Product found.");
        }

        public async Task<ApiResponse<CreateProductResponseDto>> CreateProduct(CreateProductRequestDto request, int userId)
        {
            var validator = new CreateProductRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CreateProductResponseDto>(validationResult.Errors);
            }

            if (!await IsValidBrand(request.BrandDtoId))
            {
                return new ApiResponse<CreateProductResponseDto>(null, false, "The specified brand does not exist.");
            }

            if (!await IsValidCategory(request.CategoryDtoId))
            {
                return new ApiResponse<CreateProductResponseDto>(null, false, "The specified category does not exist.");
            }

            if (!await IsValidUnitOfMeasure(request.UnitOfMeasureDtoId))
            {
                return new ApiResponse<CreateProductResponseDto>(null, false, "The specified unit of measure does not exist.");
            }

            var status = Enum.Parse<ProductStatus>(request.Status, true);

            var normalizedName = GeneralUtils.NormalizeName(request.Name);
            var productCode = await GenerateProductCodeAsync(request.Name);

            var product = new Product 
            { 
                Code = productCode,
                Name = request.Name,
                NormalizedName = normalizedName,
                GenericName = request.GenericName,
                Description = request.Description,
                CostPrice = request.CostPrice,
                SellingPrice = request.SellingPrice,
                Status = status,
                Notes = request.Notes,

                BrandId = request.BrandDtoId,
                CategoryId = request.CategoryDtoId,
                UnitOfMeasureId = request.UnitOfMeasureDtoId,
                CreatedBy = userId
            };

            await _unitOfWork.ProductRepository.CreateAsync(product);
            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<CreateProductResponseDto>(null, false, "An error occurred while creating the product.");
            }

            var brandName = await _unitOfWork.BrandRepository
                .FindByConditionAsync(x => x.BrandId == product.BrandId)
                .Select(b => b.Name)
                .FirstOrDefaultAsync();

            var categoryName = await _unitOfWork.CategoryRepository
                .FindByConditionAsync(x => x.CategoryId == product.CategoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            var unitOfMeasureName = await _unitOfWork.UnitOfMeasureRepository
                .FindByConditionAsync(x => x.UnitOfMeasureId == product.UnitOfMeasureId)
                .Select(u =>  u.Name)
                .FirstOrDefaultAsync();

            var createdByName = await _userManager.GetFullNameByIdAsync(userId);

            var createdProduct = new CreateProductResponseDto
            {
                Code = product.Code,
                Name = product.Name,
                NormalizedName = product.NormalizedName,
                GenericName = product.GenericName,
                Description = product.Description,
                CostPrice = product.CostPrice,
                SellingPrice = product.SellingPrice,
                Status = ProductStatusUtils.GetProductStatusDisplayName(product.Status),
                ImageUrl = product.ImageUrl,
                Notes = product.Notes,

                BrandId = product.BrandId,
                BrandName = brandName,
                CategoryId = product.CategoryId,
                CategoryName = categoryName,
                UnitOfMeasureId = product.UnitOfMeasureId,
                UnitOfMeasureName = unitOfMeasureName,
                CreatedByName = createdByName,
            };

            return new ApiResponse<CreateProductResponseDto>(createdProduct, true, "Product created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateProduct(UpdateProductRequestDto request, int productId, int userId)
        {
            var validator = new UpdateProductRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<string>(validationResult.Errors);
            }

            var normalizedName = GeneralUtils.NormalizeName(request.Name);

            var products = await _unitOfWork.ProductRepository
                .FindByConditionWithTrackingAsync(x => x.ProductId == productId || x.NormalizedName == normalizedName)
                .ToListAsync();

            var product = products.FirstOrDefault(x => x.ProductId == productId);
            if (product.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Product not found.");
            }

            var isNameExisting = products.Any(x => x.ProductId != productId && x.NormalizedName == normalizedName);
            if (isNameExisting)
            {
                return new ApiResponse<string>(null, false, "Product with this name already exists.");
            }

            if (!await IsValidBrand(request.BrandDtoId))
            {
                return new ApiResponse<string>(null, false, "The specified brand does not exist.");
            }

            if (!await IsValidCategory(request.CategoryDtoId))
            {
                return new ApiResponse<string>(null, false, "The specified category does not exist.");
            }

            if (!await IsValidUnitOfMeasure(request.UnitOfMeasureDtoId))
            {
                return new ApiResponse<string>(null, false, "The specified unit of measure does not exist.");
            }

            ProductStatus status = Enum.Parse<ProductStatus>(request.Status, true);

            product.Name = request.Name;
            product.NormalizedName = normalizedName;
            product.GenericName = request.GenericName;
            product.Description = request.Description;
            product.CostPrice = request.CostPrice;
            product.SellingPrice = request.SellingPrice;
            product.Status = status;
            product.Notes = request.Notes;

            product.BrandId = request.BrandDtoId;
            product.CategoryId = request.CategoryDtoId;
            product.UnitOfMeasureId = request.UnitOfMeasureDtoId;
            product.UpdatedBy = userId;
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.ProductRepository.Update(product);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while updating the product.");
            }

            return new ApiResponse<string>(null, true, "The product updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteProduct(int productId, int userId)
        {
            var product = await _unitOfWork.ProductRepository
                .FindByConditionWithTrackingAsync(x => x.ProductId == productId)
                .FirstOrDefaultAsync();

            if (product.HasNoValue())
            {
                return new ApiResponse<string>(null, false, "Product not found.");
            }

            product.UpdatedBy = userId;
            product.UpdatedAt = DateTime.Now;
            _unitOfWork.ProductRepository.SoftDelete(product);

            if (!await _unitOfWork.CommitAsync())
            {
                return new ApiResponse<string>(null, false, "An error occurred while deleting the product.");
            }

            return new ApiResponse<string>(null, true, "Product deleted successfully.");
        }

        #region Helper methods for Product

        /// <summary>
        /// Sorts the provided <see cref="IQueryable{Product}"/> based on the specified sorting criteria.
        /// The sorting criteria are provided as a string in the format "property direction", 
        /// where "direction" can be "asc" for ascending or "desc" for descending. 
        /// Multiple criteria can be specified, separated by commas, e.g., "name desc, code asc".
        /// If no valid criteria are specified, the default sorting is by BrandId in descending order.
        /// </summary>
        /// <param name="productsQuery">The queryable collection of <see cref="Product"/> to be sorted.</param>
        /// <param name="sortBy">A string containing sorting criteria, e.g., "name desc, code asc".</param>
        /// <returns>An <see cref="IQueryable{Product}"/> sorted based on the specified criteria.</returns>
        private IQueryable<Product> SortProductsQuery(IQueryable<Product> productsQuery, string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                // Default sorting
                return productsQuery.OrderByDescending(x => x.ProductId);
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
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.ProductId)
                        : productsQuery.OrderBy(x => x.ProductId);
                }

                if (propertyName.ToLower() == "code")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.Code)
                        : productsQuery.OrderBy(x => x.Code);
                }

                if (propertyName.ToLower() == "name")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.Name)
                        : productsQuery.OrderBy(x => x.Name);
                }

                if (propertyName.ToLower() == "GenericName")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.GenericName)
                        : productsQuery.OrderBy(x => x.GenericName);
                }

                if (propertyName.ToLower() == "Description")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.Description)
                        : productsQuery.OrderBy(x => x.Description);
                }

                if (propertyName.ToLower() == "CostPrice")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.CostPrice)
                        : productsQuery.OrderBy(x => x.CostPrice);
                }

                if (propertyName.ToLower() == "SellingPrice")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.SellingPrice)
                        : productsQuery.OrderBy(x => x.SellingPrice);
                }

                if (propertyName.ToLower() == "Status")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.Status)
                        : productsQuery.OrderBy(x => x.Status);
                }

                if (propertyName.ToLower() == "Notes")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.Notes)
                        : productsQuery.OrderBy(x => x.Notes);
                }

                if (propertyName.ToLower() == "BrandId")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.BrandId)
                        : productsQuery.OrderBy(x => x.BrandId);
                }

                if (propertyName.ToLower() == "BrandId")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.BrandId)
                        : productsQuery.OrderBy(x => x.BrandId);
                }

                if (propertyName.ToLower() == "CategoryId")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.CategoryId)
                        : productsQuery.OrderBy(x => x.CategoryId);
                }

                if (propertyName.ToLower() == "UnitOfMeasureId")
                {
                    productsQuery = orderDescending
                        ? productsQuery.OrderByDescending(x => x.UnitOfMeasureId)
                        : productsQuery.OrderBy(x => x.UnitOfMeasureId);
                }
            }

            return productsQuery;
        }

        /// <summary>
        /// Generates a unique product code based on the product name, consisting of the first three uppercase letters of the name and a random 4-digit numeric string.
        /// </summary>
        /// <param name="productName">The name of the product for which to generate the product code.</param>
        /// <returns>A task that represents the asynchronous operation, containing the generated product code in the format "PRD-XXX-####".</returns>
        public static Task<string> GenerateProductCodeAsync(string productName)
        {
            return Task.Run(() =>
            {
                string processedName = new string(productName
                    .ToUpper()
                    .Where(char.IsLetterOrDigit)
                    .ToArray());

                if (processedName.Length > 3)
                {
                    processedName = processedName.Substring(0, 3);
                }
                else
                {
                    processedName = processedName.PadRight(3, 'X');
                }

                // Generate a random 4-digit numeric component.
                var random = new Random();
                int randomNumber = random.Next(1000, 9999);

                string productCode = $"PRD-{processedName}-{randomNumber}";

                return productCode;
            });
        }

        private async Task<bool> IsValidBrand(int brandId)
        {
            return await _unitOfWork.BrandRepository
                .FindByConditionAsync(x => x.BrandId == brandId)
                .AnyAsync();
        }

        /// <summary>
        /// Checks if the category with the specified ID exists in the database.
        /// </summary>
        /// <param name="CategoryId">The ID of the brand to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the category exists.</returns>

        private async Task<bool> IsValidCategory(int CategoryId)
        {
            return await _unitOfWork.CategoryRepository
                .FindByConditionAsync(x => x.CategoryId == CategoryId)
                .AnyAsync();
        }

        /// <summary>
        /// Checks if the unitOfMeasure with the specified ID exists in the database.
        /// </summary>
        /// <param name="unitOfMeasureId">The ID of the brand to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the unitOfMeasure exists.</returns>

        private async Task<bool> IsValidUnitOfMeasure(int unitOfMeasureId)
        {
            return await _unitOfWork.UnitOfMeasureRepository
                .FindByConditionAsync(x => x.UnitOfMeasureId == unitOfMeasureId)
                .AnyAsync();
        }

        #endregion
    }
}
