using Domain.DTOs;
using Domain;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkTasks.Queries
{
    public class GetMyTasksQueryHandler : IRequestHandler<GetMyTasksQuery, List<WorkTaskDTO>>
    {
        private readonly AppDBContext _context;

        public GetMyTasksQueryHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<WorkTaskDTO>> Handle(GetMyTasksQuery request, CancellationToken cancellationToken)
        {
            var workTasks = await _context.WorkTasks
                .Where(t => t.AssignedToUserID == request.UserId)
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .OrderByDescending(t => t.DueDate)
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
