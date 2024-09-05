﻿namespace MedicineShopApplication.DLL.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public Cart Cart { get; set; }
        public List<Order> Orders { get; set; }
    }
}