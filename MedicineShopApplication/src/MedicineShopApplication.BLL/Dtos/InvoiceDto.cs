namespace MedicineShopApplication.BLL.Dtos
{
    public class InvoiceDto : BaseEntityDto
    {
        public int InvoiceDtoId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        public int OrderDtoId { get; set; }
        public OrderDto OrderDto { get; set; }
    }
}
