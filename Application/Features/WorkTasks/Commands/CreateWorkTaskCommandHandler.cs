using Domain.DTOs;
using Domain;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.WorkTasks.Commands
{
    public class CreateWorkTaskCommandHandler : IRequestHandler<CreateWorkTaskCommand, WorkTaskDTO>
    {
        private readonly AppDBContext _context;
        private readonly ILogger<CreateWorkTaskCommandHandler> _logger;

        public CreateWorkTaskCommandHandler(AppDBContext context, ILogger<CreateWorkTaskCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<WorkTaskDTO> Handle(CreateWorkTaskCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new work task: {Title} for team {TeamId} assigned to {UserId}", 
                request.Title, request.TeamId, request.AssignedToUserID);

            var workTask = new WorkTask
            {
                Title = request.Title,
                Description = request.Description,
                StatusID = request.StatusID,
                TeamId = request.TeamId,
                AssignedToUserID = request.AssignedToUserID,
                DueDate = request.DueDate,
                CreatedByUserID = request.AssignedToUserID
            };

            _context.WorkTasks.Add(workTask);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Work task created successfully with ID: {TaskId}", workTask.Id);

            var task = await _context.WorkTasks
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .FirstOrDefaultAsync(t => t.Id == workTask.Id, cancellationToken);

            return MapToDTO(task);
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
