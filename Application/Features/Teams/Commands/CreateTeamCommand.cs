using Domain.DTOs;
using MediatR;

namespace Application.Features.Teams.Commands
{
    public class CreateTeamCommand : IRequest<TeamDTO>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
