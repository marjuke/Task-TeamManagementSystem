using MediatR;

namespace Application.Features.Teams.Commands
{
    public class DeleteTeamCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteTeamCommand(int id)
        {
            Id = id;
        }
    }
}
