using Domain.DTOs;
using Domain;
using MediatR;
using Persistance;

namespace Application.Features.Teams.Commands
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, TeamDTO>
    {
        private readonly AppDBContext _context;

        public CreateTeamCommandHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<TeamDTO> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = new Team
            {
                Name = request.Name,
                Description = request.Description
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync(cancellationToken);

            return new TeamDTO
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                Tasks = new()
            };
        }
    }
}
