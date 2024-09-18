using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace MedicineShopApplication.BLL.Dtos.Common
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }

        public ApiResponse(T data, bool isSuccess = true, string message = "") {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
            Errors = null;
        }

        public ApiResponse(List<ValidationFailure> failures)
        {
            IsSuccess = false;
            Message = "Validation failure happened.";
            Errors = failures.GroupBy(f => f.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        public ApiResponse(IEnumerable<IdentityError> errors)
        {
            IsSuccess = false;
            Message = "Identity error happened.";
            Errors = errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description
                ).ToArray());
        }
    }
}
