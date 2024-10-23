using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
    [Table("recoveryhistorys", Schema = "public")]
    public class RecoveryHistory
    {
        public int id { get; set; }
        public string description { get; set; }
        public int employee_id { get; set; }
        public DateTimeOffset recovery_date { get; set; }
        public DateTimeOffset total_time { get; set; }
        public int object_id { get; set; }
    }
}
