using Domain.DTOs;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Teams.Queries
{
    public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, List<TeamDTO>>
    {
        private readonly AppDBContext _context;

        public GetAllTeamsQueryHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<TeamDTO>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Teams
                .Include(t => t.Tasks)
                .Select(t => new TeamDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    Tasks = t.Tasks.Select(task => new WorkTaskDTO
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        StatusID = task.StatusID,
                        TeamId = task.TeamId,
                        DueDate = task.DueDate
                    }).ToList()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
