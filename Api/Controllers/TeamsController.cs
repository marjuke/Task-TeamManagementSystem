
using Domain.DTOs;
using Application.Features.Teams.Commands;
using Application.Features.Teams.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController : BaseApiController
    {
        /// <summary>
        /// Get all teams
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TeamDTO>>> GetAllTeams()
        {
            var result = await Mediator.Send(new GetAllTeamsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get team by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeamById(int id)
        {
            var result = await Mediator.Send(new GetTeamByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Create a new team (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminCanManageTeams")]
        public async Task<ActionResult<TeamDTO>> CreateTeam([FromBody] CreateTeamDTO createTeamDTO)
        {
            var command = new CreateTeamCommand
            {
                Name = createTeamDTO.Name,
                Description = createTeamDTO.Description
            };

            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetTeamById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing team (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminCanManageTeams")]
        public async Task<ActionResult<TeamDTO>> UpdateTeam(int id, [FromBody] UpdateTeamDTO updateTeamDTO)
        {
            var command = new UpdateTeamCommand
            {
                Id = id,
                Name = updateTeamDTO.Name,
                Description = updateTeamDTO.Description
            };

            var result = await Mediator.Send(command);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Delete a team (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var result = await Mediator.Send(new DeleteTeamCommand(id));
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
