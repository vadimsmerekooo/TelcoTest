using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelcoTestAsp.Models
{
    public class Task
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start_date { get; set; } = DateTime.Now;
        public DateTime End_date { get; set; } = DateTime.Now.AddDays(1);
        public TaskStatus Status { get; set; }

        public List<TaskElement> TaskElements { get; set; }
    }
}
