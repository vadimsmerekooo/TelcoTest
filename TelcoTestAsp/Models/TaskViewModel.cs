using System.Collections.Generic;

namespace TelcoTestAsp.Models
{
    public class TaskViewModel
    {
        public Task Task { get; set; }
        public List<TaskElement> TaskElements { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
