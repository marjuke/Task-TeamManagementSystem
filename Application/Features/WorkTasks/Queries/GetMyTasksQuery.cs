using Domain.DTOs;
using MediatR;

namespace Application.Features.WorkTasks.Queries
{
    public class GetMyTasksQuery : IRequest<List<WorkTaskDTO>>
    {
        public required string UserId { get; set; }
    }
}
