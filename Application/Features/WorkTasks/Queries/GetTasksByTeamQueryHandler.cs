using Domain.DTOs;
using Domain;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkTasks.Queries
{
    public class GetTasksByTeamQueryHandler : IRequestHandler<GetTasksByTeamQuery, List<WorkTaskDTO>>
    {
        private readonly AppDBContext _context;

        public GetTasksByTeamQueryHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<WorkTaskDTO>> Handle(GetTasksByTeamQuery request, CancellationToken cancellationToken)
        {
            var workTasks = await _context.WorkTasks
                .Where(w => w.TeamId == request.TeamId)
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .ToListAsync(cancellationToken);

            return workTasks.Select(MapToDTO).ToList();
        }

        private WorkTaskDTO MapToDTO(WorkTask workTask)
        {
            return new WorkTaskDTO
            {
                Id = workTask.Id,
                Title = workTask.Title,
                Description = workTask.Description,
                StatusID = workTask.StatusID,
                StatusName = workTask.Status?.Name,
                TeamId = workTask.TeamId,
                TeamName = workTask.Team?.Name,
                AssignedToUserID = workTask.AssignedToUserID,
                AssignedToUserName = workTask.AssignedToUser?.UserName,
                CreatedByUserID = workTask.CreatedByUserID,
                CreatedByUserName = workTask.CreatedByUser?.UserName,
                DueDate = workTask.DueDate
            };
        }
    }
}
