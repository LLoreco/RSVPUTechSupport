﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
        public DateTime send_time { get; set; }
        public DateTime time_limit { get; set; }
        public DateTime total_time { get; set; }
        public string status { get; set; }
        public int employee_id { get; set; }
        public string image { get; set; }

        // Навигационное свойство для связи с сотрудником
        [ForeignKey("from_whom_id")]
        public Employee? FromWhom { get; set; }

        // Навигационное свойство для связи с сотрудником
        [ForeignKey("employee_id")]
        public Employee? Employee { get; set; }

    }
}