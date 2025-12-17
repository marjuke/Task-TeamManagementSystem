using MediatR;

namespace Application.Features.WorkTasks.Commands
{
    public class DeleteWorkTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteWorkTaskCommand(int id)
        {
            Id = id;
        }
    }
}
