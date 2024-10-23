using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace API.Data
{
    [Table("tasks", Schema = "public")]
    public class Work
    {
        [Key]
        public int id { get; set; }
        public int work_number { get; set; }
        public int from_whom_id { get; set; }
        public string description { get; set; }
        public DateTimeOffset send_time { get; set; }
        public DateTimeOffset time_limit { get; set; }
        public DateTimeOffset total_time { get; set; }
        public string status { get; set; }
        public int employee_id { get; set; }
        public string image { get; set; }
        public int object_id { get; set; }

        // Навигационное свойство для связи с сотрудником
        [ForeignKey("from_whom_id")]
        [NotMapped]
        public Employee? FromWhom { get; set; }

        // Навигационное свойство для связи с сотрудником
        [ForeignKey("employee_id")]
        [NotMapped]
        public Employee? Employee { get; set; }

    }
}
