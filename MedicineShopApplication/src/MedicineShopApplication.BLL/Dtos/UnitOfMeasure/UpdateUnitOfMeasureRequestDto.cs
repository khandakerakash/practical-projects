namespace MedicineShopApplication.BLL.Dtos.UnitOfMeasure
{
    public class UpdateUnitOfMeasureRequestDto
    {
        public int UnitOfMeasureDtoId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
    }
}
