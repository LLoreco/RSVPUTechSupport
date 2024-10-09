using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
    [Table("recoveryhistorys", Schema = "public")]
    public class RecoveryHistory
    {
        public int id { get; set; }
        public string description { get; set; }
        public int employee_id { get; set; }
        public DateTime recovery_date { get; set; }
        public DateTime total_time { get; set; }
        public int object_id { get; set; }
    }
}
