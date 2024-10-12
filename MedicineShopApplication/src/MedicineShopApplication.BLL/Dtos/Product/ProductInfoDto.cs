namespace MedicineShopApplication.BLL.Dtos.Product
{
    public class ProductInfoDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
    }
}
