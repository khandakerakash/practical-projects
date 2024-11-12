using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedicineShopApplication.DLL.Models.Common
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public int ClientId { set; get; }

        public AuditLog ToAuditLog()
        {
            var auditLog = new AuditLog
            {
                TableName = TableName,
                Action = Action,
                Timestamp = Timestamp,
                KeyValues = JsonSerializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
                UserName = UserName,
                ControllerName = ControllerName,
                ActionName = ActionName
            };
            return auditLog;
        }
    }
}
