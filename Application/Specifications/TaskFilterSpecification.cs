using Domain;
using System;

namespace Application.Specifications
{
    public class TaskFilterSpecification
    {
        public int? StatusID { get; set; }
        public string? AssignedToUserID { get; set; }
        public int? TeamId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }

        /// <summary>
        /// Applies all filters to an IQueryable of WorkTasks
        /// </summary>
        public IQueryable<WorkTask> ApplyFilters(IQueryable<WorkTask> query)
        {
            if (StatusID.HasValue)
                query = query.Where(t => t.StatusID == StatusID.Value);

            if (!string.IsNullOrWhiteSpace(AssignedToUserID))
                query = query.Where(t => t.AssignedToUserID == AssignedToUserID);

            if (TeamId.HasValue)
                query = query.Where(t => t.TeamId == TeamId.Value);

            if (DueDateFrom.HasValue)
                query = query.Where(t => t.DueDate >= DueDateFrom.Value);

            if (DueDateTo.HasValue)
                query = query.Where(t => t.DueDate <= DueDateTo.Value);

            return query;
        }

        /// <summary>
        /// Checks if any filters are applied
        /// </summary>
        public bool HasAnyFilter()
        {
            return StatusID.HasValue || 
                   !string.IsNullOrWhiteSpace(AssignedToUserID) || 
                   TeamId.HasValue || 
                   DueDateFrom.HasValue || 
                   DueDateTo.HasValue;
        }

        /// <summary>
        /// Gets a summary of applied filters for logging
        /// </summary>
        public Dictionary<string, object?> GetFilterSummary()
        {
            var summary = new Dictionary<string, object?>();

            if (StatusID.HasValue)
                summary["StatusID"] = StatusID.Value;
            if (!string.IsNullOrWhiteSpace(AssignedToUserID))
                summary["AssignedToUserID"] = AssignedToUserID;
            if (TeamId.HasValue)
                summary["TeamId"] = TeamId.Value;
            if (DueDateFrom.HasValue)
                summary["DueDateFrom"] = DueDateFrom.Value;
            if (DueDateTo.HasValue)
                summary["DueDateTo"] = DueDateTo.Value;

            return summary;
        }
    }
}
