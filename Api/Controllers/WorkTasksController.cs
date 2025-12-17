using Domain.DTOs;
using Application.Features.WorkTasks.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.WorkTasks.Queries;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkTasksController : BaseApiController
    {
        /// <summary>
        /// Get all tasks
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<WorkTaskDTO>>> GetAllTasks()
        {
            var result = await Mediator.Send(new GetAllTasksQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get task by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkTaskDTO>> GetTaskById(int id)
        {
            var result = await Mediator.Send(new GetTaskByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Get tasks by team id
        /// </summary>
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<List<WorkTaskDTO>>> GetTasksByTeam(int teamId)
        {
            var result = await Mediator.Send(new GetTasksByTeamQuery(teamId));
            return Ok(result);
        }

        /// <summary>
        /// Search and filter tasks with pagination
        /// </summary>
        [HttpPost("search")]
        public async Task<ActionResult<PaginatedResponse<WorkTaskDTO>>> SearchTasks([FromBody] WorkTaskFilterDTO filterDTO)
        {
            var query = new GetFilteredTasksQuery
            {
                StatusID = filterDTO.StatusID,
                AssignedToUserID = filterDTO.AssignedToUserID,
                TeamId = filterDTO.TeamId,
                DueDateFrom = filterDTO.DueDateFrom,
                DueDateTo = filterDTO.DueDateTo,
                PageNumber = filterDTO.PageNumber,
                PageSize = filterDTO.PageSize
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Create a new task (Manager or above)
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "ManagerOrAdmin")]
        public async Task<ActionResult<WorkTaskDTO>> CreateTask([FromBody] CreateWorkTaskDTO createTaskDTO)
        {
            var command = new CreateWorkTaskCommand
            {
                Title = createTaskDTO.Title,
                Description = createTaskDTO.Description,
                StatusID = createTaskDTO.StatusID,
                TeamId = createTaskDTO.TeamId,
                AssignedToUserID = createTaskDTO.AssignedToUserID,
                DueDate = createTaskDTO.DueDate
            };

            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing task (Manager or above)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOrAdmin")]
        public async Task<ActionResult<WorkTaskDTO>> UpdateTask(int id, [FromBody] UpdateWorkTaskDTO updateTaskDTO)
        {
            var command = new UpdateWorkTaskCommand
            {
                Id = id,
                Title = updateTaskDTO.Title,
                Description = updateTaskDTO.Description,
                StatusID = updateTaskDTO.StatusID,
                TeamId = updateTaskDTO.TeamId,
                AssignedToUserID = updateTaskDTO.AssignedToUserID,
                DueDate = updateTaskDTO.DueDate
            };

            var result = await Mediator.Send(command);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Delete a task (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await Mediator.Send(new DeleteWorkTaskCommand(id));
            if (!result)
                return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Get tasks assigned to the current user (Employee)
        /// </summary>
        [HttpGet("my-tasks")]
        public async Task<ActionResult<List<WorkTaskDTO>>> GetMyTasks()
        {
            var userId = GetCurrentUserId();
            var result = await Mediator.Send(new GetMyTasksQuery { UserId = userId });
            return Ok(result);
        }

        /// <summary>
        /// Update task status for assigned task (Employee)
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<WorkTaskDTO>> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDTO updateStatusDTO)
        {
            var userId = GetCurrentUserId();
            var command = new UpdateTaskStatusCommand
            {
                Id = id,
                StatusID = updateStatusDTO.StatusID,
                UserId = userId
            };

            try
            {
                var result = await Mediator.Send(command);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
