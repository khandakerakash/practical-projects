using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MedicineShopApplication.DLL.UOW;
using MedicineShopApplication.BLL.Dtos.Common;
using MedicineShopApplication.BLL.Validations;
using MedicineShopApplication.DLL.Models.Users;
using MedicineShopApplication.BLL.Dtos.Product;
using MedicineShopApplication.DLL.Models.General;


namespace MedicineShopApplication.BLL.Services
{
    public interface IProductService
    {
        Task<ApiResponse<CreateProductResponseDto>> CreateProduct(CreateProductRequestDto request, int userId);
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

        public async Task<ApiResponse<CreateProductResponseDto>> CreateProduct(CreateProductRequestDto request, int userId)
        {
            var validator = new CreateProductRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<CreateProductResponseDto>(validationResult.Errors);
            }


            var product = new Product() 
            { 
                Code = request.Code,
                Name = request.Name,
                GenericName = request.GenericName,
                Description = request.Description,
                BrandId = request.BrandDtoId,
                CostPrice = request.CostPrice,
                SellingPrice = request.SellingPrice,
                //UnitOfMeasure = request.UnitOfMeasure,
                ImageUrl = request.ImageUrl,
                Notes = request.Notes,
            };


            var user = await _userManager.FindByIdAsync(userId.ToString());
            var createdByName = user != null
                ? $"{(user.Title ?? "")} {(user.FirstName ?? "")} {(user.LastName ?? "")}".Trim()
                : "Unknown User";

            return null;
        }

        #region Helper methods for Create Product

        /// <summary>
        /// Generates a unique Product code by appending an incremented numeric suffix to the provided prefix.
        /// This method checks the database for existing codes with the same prefix and increments the suffix
        /// until a unique code is generated. The final code will be in the format "PREFIX-####".
        /// </summary>
        /// <param name="prefix">The prefix derived from the Product name.</param>
        /// <returns>A unique Product code in the format "PREFIX-####".</returns>
        public async Task<string> GenerateUniqueProductCodeAsync(string productName)
        {
            string prefix = GeneratePrefixFromProductName(productName);

            var latestCategory = await _unitOfWork.ProductRepository
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
        /// Generates a prefix for a Product code based on the given Product name.
        /// 
        /// The prefix is generated as follows:
        /// - If the first word contains an alphanumeric character combination (e.g., "H2"), it uses that and appends the first letter of the second word (if available).
        /// - For single-word names, it uses the first three letters of the word.
        /// - For two-word names, it combines the first letter of each word.
        /// - For three or more words, it combines the first letter of the first three words.
        /// 
        /// Special characters are removed, and the result is returned in uppercase.
        /// </summary>
        /// <param name="productName">The Product name from which to generate the prefix.</param>
        /// <returns>A prefix string based on the Product name.</returns>

        private string GeneratePrefixFromProductName(string productName)
        {
            string cleanedName = new string(productName.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
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
    }
}
