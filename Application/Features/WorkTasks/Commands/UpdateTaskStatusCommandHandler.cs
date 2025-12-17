using Domain.DTOs;
using Domain;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkTasks.Commands
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, WorkTaskDTO>
    {
        private readonly AppDBContext _context;

        public UpdateTaskStatusCommandHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<WorkTaskDTO> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var workTask = await _context.WorkTasks
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (workTask == null)
                return null;

            if (workTask.AssignedToUserID != request.UserId)
                throw new UnauthorizedAccessException("You can only update status for tasks assigned to you.");

            workTask.StatusID = request.StatusID;

            _context.WorkTasks.Update(workTask);
            await _context.SaveChangesAsync(cancellationToken);

            var updatedTask = await _context.WorkTasks
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            return MapToDTO(updatedTask);
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
