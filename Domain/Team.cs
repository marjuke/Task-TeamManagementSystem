using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
    }
}
