using Domain.DTOs;
using MediatR;

namespace Application.Features.WorkTasks.Queries
{
    public class GetTasksByTeamQuery : IRequest<List<WorkTaskDTO>>
    {
        public int TeamId { get; set; }

        public GetTasksByTeamQuery(int teamId)
        {
            TeamId = teamId;
        }
    }
}
