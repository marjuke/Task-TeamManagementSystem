using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
    }
}
