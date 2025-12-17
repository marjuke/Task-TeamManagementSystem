using Domain;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class UsersController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<User> userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Get all users (Admin only)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            _logger.LogInformation("Admin retrieving all users");

            var users = await _userManager.Users.ToListAsync();
            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDTOs.Add(new UserDTO
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName!,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    Image = user.Image,
                    Roles = roles,
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty
                });
            }

            return Ok(userDTOs);
        }

        /// <summary>
        /// Get user by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(string id)
        {
            _logger.LogInformation("Admin retrieving user: {UserId}", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", id);
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserDTO
            {
                Id = user.Id,
                DisplayName = user.DisplayName!,
                Email = user.Email!,
                UserName = user.UserName!,
                Image = user.Image,
                Roles = roles,
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            });
        }

        /// <summary>
        /// Update user (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(string id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            _logger.LogInformation("Admin updating user: {UserId}", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for update: {UserId}", id);
                return NotFound();
            }

            user.DisplayName = updateUserDTO.DisplayName ?? user.DisplayName;
            user.Email = updateUserDTO.Email ?? user.Email;
            user.UserName = updateUserDTO.Email ?? user.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to update user: {UserId}. Errors: {Errors}", id, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserDTO
            {
                Id = user.Id,
                DisplayName = user.DisplayName!,
                Email = user.Email!,
                UserName = user.UserName!,
                Image = user.Image,
                Roles = roles,
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            });
        }

        /// <summary>
        /// Delete user (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            _logger.LogInformation("Admin deleting user: {UserId}", id);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for deletion: {UserId}", id);
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to delete user: {UserId}. Errors: {Errors}", id, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("User deleted successfully: {UserId}", id);
            return NoContent();
        }

        /// <summary>
        /// Change user role (Admin only)
        /// </summary>
        [HttpPut("{id}/role")]
        public async Task<ActionResult<UserDTO>> ChangeUserRole(string id, [FromBody] ChangeUserRoleDTO changeRoleDTO)
        {
            _logger.LogInformation("Admin changing role for user: {UserId} to {Role}", id, changeRoleDTO.Role);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for role change: {UserId}", id);
                return NotFound();
            }

            // Remove existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add new role
            var result = await _userManager.AddToRoleAsync(user, changeRoleDTO.Role);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to change user role: {UserId}. Errors: {Errors}", id, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserDTO
            {
                Id = user.Id,
                DisplayName = user.DisplayName!,
                Email = user.Email!,
                UserName = user.UserName!,
                Image = user.Image,
                Roles = roles,
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            });
        }
    }
}
