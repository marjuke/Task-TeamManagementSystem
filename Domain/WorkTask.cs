using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class WorkTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int StatusID { get; set; }
        public Status Status { get; set; } = null!;

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public string AssignedToUserID { get; set; } = null!;
        public User AssignedToUser { get; set; } = null!;

        public string CreatedByUserID { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;

        public DateTime DueDate { get; set; }
    }
}
