using MedicineShopApplication.BLL.Dtos.Common;
using Microsoft.AspNetCore.Http;

namespace MedicineShopApplication.BLL.Services
{
    public interface IImageService
    {
        Task<ApiResponse<string>> UploadImageAsync(IFormFile imageFile, string imageCategory);
        Task<ApiResponse<string>> DeleteImageAsync(string imageUrl);
    }

    public class ImageService : IImageService
    {
        private readonly string _storagePath = Path.Combine("wwwroot", "uploads");

        public async Task<ApiResponse<string>> UploadImageAsync(IFormFile imageFile, string imageCategory)
        {
            if (imageFile == null || imageFile.Length == 0) 
            {
                return new ApiResponse<string>(null, false, "Invalid image file.");
            }

            if(!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_storagePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var imageURL = Path.Combine("uploads", fileName).Replace("\\", "/");

            return new ApiResponse<string>(imageURL, true, "Image inserted successfully.");
        }

        public Task<ApiResponse<string>> DeleteImageAsync(string imageUrl)
        {
            var filePath = Path.Combine("wwwroot", imageUrl);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                //return Task.FromResult(true);
                //return new ApiResponse<string>(null, true, "Image deleted successfully.");
            }

            //return new ApiResponse<string>(null, false, "An error occurred while deleting the image.");
            return null;
        }
    }
}
