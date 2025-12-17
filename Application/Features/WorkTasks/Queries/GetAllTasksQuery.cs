using Domain.DTOs;
using MediatR;

namespace Application.Features.WorkTasks.Queries
{
    public class GetAllTasksQuery : IRequest<List<WorkTaskDTO>>
    {
    }
}
