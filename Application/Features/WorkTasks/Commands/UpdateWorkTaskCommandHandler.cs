using Domain.DTOs;
using Domain;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkTasks.Commands
{
    public class UpdateWorkTaskCommandHandler : IRequestHandler<UpdateWorkTaskCommand, WorkTaskDTO>
    {
        private readonly AppDBContext _context;

        public UpdateWorkTaskCommandHandler(AppDBContext context)
        {
            _context = context;
        }

        public async Task<WorkTaskDTO> Handle(UpdateWorkTaskCommand request, CancellationToken cancellationToken)
        {
            var workTask = await _context.WorkTasks
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (workTask == null)
                return null;

            workTask.Title = request.Title;
            workTask.Description = request.Description;
            workTask.StatusID = request.StatusID;
            workTask.TeamId = request.TeamId;
            workTask.AssignedToUserID = request.AssignedToUserID;
            workTask.DueDate = request.DueDate;

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
