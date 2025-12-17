using Domain.DTOs;
using MediatR;

namespace Application.Features.Teams.Commands
{
    public class UpdateTeamCommand : IRequest<TeamDTO?>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
