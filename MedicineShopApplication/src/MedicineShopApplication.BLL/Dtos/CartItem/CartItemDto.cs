﻿using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Cart;
using MedicineShopApplication.BLL.Dtos.Product;

namespace MedicineShopApplication.BLL.Dtos.CartItem
{
    public class CartItemDto : AuditableEntityDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }

        public int CartId { get; set; }
        public CartDto CartDto { get; set; }
        public int ProductId { get; set; }
        public ProductDto ProductDtos { get; set; }
    }
}
