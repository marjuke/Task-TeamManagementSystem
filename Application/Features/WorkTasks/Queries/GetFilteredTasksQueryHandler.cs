using Domain.DTOs;
using Domain;
using Domain.Enums;
using MediatR;
using Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Specifications;

namespace Application.Features.WorkTasks.Queries
{
    public class GetFilteredTasksQueryHandler : IRequestHandler<GetFilteredTasksQuery, PaginatedResponse<WorkTaskDTO>>
    {
        private readonly AppDBContext _context;
        private readonly ILogger<GetFilteredTasksQueryHandler> _logger;

        public GetFilteredTasksQueryHandler(AppDBContext context, ILogger<GetFilteredTasksQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedResponse<WorkTaskDTO>> Handle(GetFilteredTasksQuery request, CancellationToken cancellationToken)
        {
            // Validate pagination parameters
            if (request.PageNumber < 1)
                request.PageNumber = 1;
            if (request.PageSize < 1 || request.PageSize > 100)
                request.PageSize = 10;

            var specification = new TaskFilterSpecification
            {
                StatusID = request.StatusID,
                AssignedToUserID = request.AssignedToUserID,
                TeamId = request.TeamId,
                DueDateFrom = request.DueDateFrom,
                DueDateTo = request.DueDateTo
            };

            _logger.LogInformation(
                "Executing task search with filters: {@FilterSummary}, Page: {PageNumber}, PageSize: {PageSize}, SortBy: {SortBy}",
                specification.GetFilterSummary(), request.PageNumber, request.PageSize, request.SortBy
            );

            var query = _context.WorkTasks
                .Include(t => t.Status)
                .Include(t => t.Team)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .AsQueryable();

            // Apply filters using specification
            query = specification.ApplyFilters(query);

            // Apply sorting
            query = ApplySorting(query, request.SortBy);

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);
            _logger.LogInformation("Total tasks matching filters: {TotalCount}", totalCount);

            // Apply pagination
            var workTasks = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items = workTasks.Select(MapToDTO).ToList();
            
            var response = new PaginatedResponse<WorkTaskDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            _logger.LogInformation(
                "Search completed: Found {ItemCount} tasks on page {PageNumber} of {TotalPages}",
                items.Count, request.PageNumber, response.TotalPages
            );

            return response;
        }

        private IQueryable<WorkTask> ApplySorting(IQueryable<WorkTask> query, SortBy sortBy)
        {
            return sortBy switch
            {
                SortBy.IdAsc => query.OrderBy(t => t.Id),
                SortBy.TitleAsc => query.OrderBy(t => t.Title),
                SortBy.TitleDesc => query.OrderByDescending(t => t.Title),
                SortBy.DueDateAsc => query.OrderBy(t => t.DueDate),
                SortBy.DueDateDesc => query.OrderByDescending(t => t.DueDate),
                SortBy.StatusAsc => query.OrderBy(t => t.StatusID),
                SortBy.CreatedAsc => query.OrderBy(t => t.Id),
                SortBy.CreatedDesc => query.OrderByDescending(t => t.Id),
                _ => query.OrderByDescending(t => t.Id) // Default: IdDesc
            };
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
