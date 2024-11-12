namespace MedicineShopApplication.DLL.Models.Common
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; } // "Insert", "Update", "Delete"
        public string KeyValues { get; set; } // JSON representation of primary key values
        public string OldValues { get; set; } // JSON representation of old values
        public string NewValues { get; set; } // JSON representation of new values
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; } // Optional: to store which user made the change

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
