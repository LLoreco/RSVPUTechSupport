using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
    [Table("employees", Schema = "public")]
    public class Employee
    {
        [Key]
        public int id { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string division { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public long phone { get; set; }
        public int work_amount { get; set; }
        public int salary { get; set; }
        public bool status { get; set; }
        public string password { get; set; }
        public string login { get; set; }
    }
}
