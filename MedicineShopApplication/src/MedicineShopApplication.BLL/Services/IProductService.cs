using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.BLL.Validations;
using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Common;
using Microsoft.EntityFrameworkCore;
using MedicineShopApplication.DLL.Models.General;
using static Azure.Core.HttpHeader;

namespace MedicineShopApplication.BLL.Services
{
    public interface IProductService
    {
        Task<ApiResponse<CreateProductResponseDto>> CreateProduct(CreateProductRequestDto request);
    }

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CreateProductResponseDto>> CreateProduct(CreateProductRequestDto request)
        {
            var validator = new CreateProductRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CreateProductResponseDto>(validationResult.Errors);
            }

            var generatedCode = await GenerateProductCodeAsync(request.Name);

            var product = new Product() 
            { 
                Code = generatedCode,
                Name = request.Name,
                GenericName = request.GenericName,
                Description = request.Description,
                //Brand = request.Brand,
                CostPrice = request.CostPrice,
                SellingPrice = request.SellingPrice,
                //UnitOfMeasure = request.UnitOfMeasure,
                ImageUrl = request.ImageUrl,
                Notes = request.Notes,
            };

            return null;
        }

        #region Helper methods for Create Product

        /// <summary>
        /// Generates a unique product code based on the given product name.
        /// This method dynamically creates a prefix using the first few letters of the product name:
        /// - For single-word names, it uses the first 3 letters of the word.
        /// - For multi-word names, it uses the first letter of each word, up to a maximum of 3 words.
        /// The method then checks the database to find how many products already exist with the same prefix,
        /// and appends the next available numeric suffix (starting from 0001) to ensure the generated code is unique.
        /// </summary>
        /// <param name="productName">The name of the product for which the code will be generated.</param>
        /// <returns>A unique product code in the format "PREFIX-####", where PREFIX is derived from the product name.</returns>
        private async Task<string> GenerateProductCodeAsync(string productName)
        {
            // Remove special characters like parentheses, dashes, underscores
            string cleanedName = new string(productName.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

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
            int count = await _unitOfWork.ProductRepository.GetProductCountByPrefixAsync(prefix);

            string generatedCode;
            int suffix = count + 1;
            bool isUnique;

            do
            {
                generatedCode = $"{prefix}-{suffix.ToString("D4")}"; // Format the suffix with leading zeroes (0001, 0002, etc.)
                isUnique = !(await _unitOfWork.ProductRepository
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
    }
}
