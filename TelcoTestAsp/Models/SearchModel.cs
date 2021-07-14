using System;
using System.Collections.Generic;

namespace TelcoTestAsp.Models
{
    public class SearchModel
    {
        public string Query { get; set; }
        public Task FilterSearch { get; set; }
        public TaskElement FilterSearchElements { get; set; }
    }
}
