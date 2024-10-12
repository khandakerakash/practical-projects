﻿using FluentValidation;
using MedicineShopApplication.BLL.Dtos.Cart;

namespace MedicineShopApplication.BLL.Validations
{
    public class AddToCartRequestDtoValidator : AbstractValidator<AddToCartRequestDto>
    {
        public AddToCartRequestDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than zero (0).");

            RuleFor(x => x.Quantity)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero (0).");
        }
    }
}
