namespace MedicineShopApplication.DLL.Models.Interfaces
{
    public interface ISoftDeletable
    {
        public bool IsDeletable { get; set; }
        public DateTime? DeletedAt { get; set; }

        public void Undo()
        {
            IsDeletable = false;
            DeletedAt = null;
        }
    }
}
