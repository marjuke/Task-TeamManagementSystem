using Domain.DTOs;
using MediatR;

namespace Application.Features.WorkTasks.Queries
{
    public class GetTaskByIdQuery : IRequest<WorkTaskDTO>
    {
        public int Id { get; set; }

        public GetTaskByIdQuery(int id)
        {
            Id = id;
        }
    }
}
