using Domain.DTOs;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Teams.Commands
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, TeamDTO?>
    {
        private readonly AppDBContext _context;

        public UpdateTeamCommandHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<TeamDTO?> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (team == null)
                return null;

            team.Name = request.Name;
            team.Description = request.Description;

            _context.Teams.Update(team);
            await _context.SaveChangesAsync(cancellationToken);

            return new TeamDTO
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                Tasks = team.Tasks.Select(task => new WorkTaskDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    StatusID = task.StatusID,
                    TeamId = task.TeamId,
                    DueDate = task.DueDate
                }).ToList()
            };
        }
    }
}
