namespace MedicineShopApplication.BLL.Dtos
{
    public class OrderDto : BaseEntityDto
    {
        public int OrderDtoId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public List<OrderItemDto> OrderItemDtos { get; set; }
        public int UserDtoId { get; set; }
        public UserDto UserDto { get; set; }
        public PaymentDto PaymentDto { get; set; }
        public InvoiceDto InvoiceDto { get; set; }
    }
}
