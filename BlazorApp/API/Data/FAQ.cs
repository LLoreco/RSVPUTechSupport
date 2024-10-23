using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
    [Table("faq", Schema = "public")]
    public class FAQ
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
