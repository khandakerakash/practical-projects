﻿namespace MedicineShopApplication.DLL.Models
{
    public class Category : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CategoryProduct> CategoryProducts { get; set; }
    }
}