using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using System.Security.Claims;

namespace Skill_Assessment_Portal_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admins can view all users
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Allow user to get their own profile, or admin/evaluator to get any user profile
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserId != id && currentUserRole != "Admin" && currentUserRole != "Evaluator")
            {
                return Forbid();
            }

            var user = await _userService.GetUserDtoByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")] // Only Admins can update a user's role
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] int newRoleId)
        {
            try
            {
                await _userService.UpdateUserRoleAsync(id, newRoleId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
