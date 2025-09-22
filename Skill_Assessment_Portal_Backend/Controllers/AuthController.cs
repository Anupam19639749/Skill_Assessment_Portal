using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;

namespace Skill_Assessment_Portal_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                var response = await _authService.RegisterAsync(userRegisterDto);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                // User already exists
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(userLoginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Invalid credentials
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
