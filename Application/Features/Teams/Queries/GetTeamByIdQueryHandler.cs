using Domain.DTOs;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Teams.Queries
{
    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDTO?>
    {
        private readonly AppDBContext _context;

        public GetTeamByIdQueryHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<TeamDTO?> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (team == null)
                return null;

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
