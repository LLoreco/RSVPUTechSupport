using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Components.Data
{
    [Table("objects", Schema = "public")]
    public class Objects
    {
        [Key]
        public int id { get; set; }
        public string object_name { get; set; }
        public string type { get; set; }
        public DateOnly buy_date { get; set; }
        public int break_count { get; set; }
        public DateOnly recovery_date { get; set; }
        public string room_number { get; set; }
       
    }
}
