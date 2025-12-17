using Domain.DTOs;
using MediatR;

namespace Application.Features.WorkTasks.Commands
{
    public class UpdateTaskStatusCommand : IRequest<WorkTaskDTO>
    {
        public int Id { get; set; }
        public int StatusID { get; set; }
        public required string UserId { get; set; }
    }
}
