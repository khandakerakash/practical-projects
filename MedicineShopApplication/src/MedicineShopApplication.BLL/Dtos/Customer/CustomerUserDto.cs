﻿using MedicineShopApplication.BLL.Dtos.Base;
using MedicineShopApplication.BLL.Dtos.Cart;
using MedicineShopApplication.BLL.Dtos.Order;
using MedicineShopApplication.BLL.Dtos.Payment;

namespace MedicineShopApplication.BLL.Dtos.Customer
{
    public class CustomerUserDto : AuditableEntityDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string NationalIdentityCard { get; set; }
        public string PostalCode { get; set; }
        public string PoliceStation { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Address { get; set; }

        public CartDto CartDto { get; set; }
        public List<OrderDto> OrdersDto { get; set; }
        public List<PaymentDto> PaymentsDto { get; set; }
    }
}
