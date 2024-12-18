﻿using MedicineShopApplication.BLL.Dtos.Admin;
using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Customer;
using MedicineShopApplication.BLL.Dtos.Invoice;
using MedicineShopApplication.BLL.Dtos.OrderItem;
using MedicineShopApplication.BLL.Dtos.Payment;

namespace MedicineShopApplication.BLL.Dtos.Order
{
    public class OrderDto : AuditableEntityDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public List<OrderItemDto> OrderItemsDto { get; set; }
        public int UserId { get; set; }
        public CustomerInfoDto CustomerInfoDto { get; set; }
        public PaymentDto PaymentDto { get; set; }
        public InvoiceDto InvoiceDto { get; set; }
    }
}
