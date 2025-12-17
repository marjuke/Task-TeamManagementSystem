using Domain.DTOs;
using MediatR;

namespace Application.Features.WorkTasks.Commands
{
    public class CreateWorkTaskCommand : IRequest<WorkTaskDTO>
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int StatusID { get; set; }
        public int TeamId { get; set; }
        public required string AssignedToUserID { get; set; }
        public DateTime DueDate { get; set; }
    }
}
