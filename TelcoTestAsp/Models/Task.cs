using System;

namespace TelcoTestAsp.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public TaskStatus Status { get; set; }
    }
}
