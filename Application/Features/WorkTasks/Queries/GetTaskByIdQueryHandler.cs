using Domain.DTOs;
using Domain;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkTasks.Queries
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, WorkTaskDTO>
    {
        private readonly AppDBContext _context;

        public GetTaskByIdQueryHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<WorkTaskDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var workTask = await _context.WorkTasks
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (workTask == null)
                return null;

            return MapToDTO(workTask);
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
