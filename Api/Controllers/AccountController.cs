using Domain.DTOs;
using Api.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<User> signInManager, ITokenService tokenService, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerDTO)
        {
            var user = new User
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };
            var result = await _signInManager.UserManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                // Assign Employee role by default to new users
                await _signInManager.UserManager.AddToRoleAsync(user, "Employee");

                var accessToken = _tokenService.CreateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();
                
                var refreshTokenExpirationDays = _configuration.GetValue<int>("TokenSettings:RefreshTokenExpirationDays", 7);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
                await _signInManager.UserManager.UpdateAsync(user);

                var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

                return Ok(new UserDTO
                {
                    DisplayName = user.DisplayName!,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    Id = user.Id,
                    Image = user.Image,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Roles = userRoles
                });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (result.Succeeded)
            {
                var accessToken = _tokenService.CreateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();
                
                var refreshTokenExpirationDays = _configuration.GetValue<int>("TokenSettings:RefreshTokenExpirationDays", 7);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
                await _signInManager.UserManager.UpdateAsync(user);

                var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

                return Ok(new UserDTO
                {
                    DisplayName = user.DisplayName!,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    Id = user.Id,
                    Image = user.Image,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Roles = userRoles
                });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            var result = await _tokenService.RefreshTokenAsync(refreshTokenDTO.RefreshToken);
            
            if (result.user == null)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            var userRoles = await _signInManager.UserManager.GetRolesAsync(result.user);

            return Ok(new UserDTO
            {
                DisplayName = result.user.DisplayName!,
                Email = result.user.Email!,
                UserName = result.user.UserName!,
                Id = result.user.Id,
                Image = result.user.Image,
                AccessToken = result.accessToken,
                RefreshToken = result.refreshToken,
                Roles = userRoles
            });
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _signInManager.UserManager.GetUserAsync(User);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _signInManager.UserManager.UpdateAsync(user);
                }
                await _signInManager.SignOutAsync();
            }
            return Ok();
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (User.Identity?.IsAuthenticated == false) return NoContent();
            var user = await _signInManager.UserManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            
            var accessToken = _tokenService.CreateAccessToken(user);
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);
            
            return Ok(new UserDTO
            {
                DisplayName = user.DisplayName!,
                Email = user.Email!,
                UserName = user.UserName!,
                Id = user.Id,
                Image = user.Image,
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken ?? string.Empty,
                Roles = userRoles
            });
        }
    }
}
