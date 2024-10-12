using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Order;

namespace MedicineShopApplication.BLL.Validations
{
    public class CreateOrderRequestDtoValidator : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderRequestDtoValidator() 
        {
            RuleFor(x => x.DeliveryAddress)
                .NotNull()
                .NotEmpty();
        }
    }
}
