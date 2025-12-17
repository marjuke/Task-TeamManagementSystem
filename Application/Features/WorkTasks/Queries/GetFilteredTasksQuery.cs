using Domain.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Features.WorkTasks.Queries
{
    public class GetFilteredTasksQuery : IRequest<PaginatedResponse<WorkTaskDTO>>
    {
        public int? StatusID { get; set; }
        public string? AssignedToUserID { get; set; }
        public int? TeamId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortBy SortBy { get; set; } = SortBy.IdDesc;
    }
}
