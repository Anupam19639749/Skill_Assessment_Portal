using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using System.Security.Claims;

namespace Skill_Assessment_Portal_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAssessmentsController : ControllerBase
    {
        private readonly IUserAssessmentService _userAssessmentService;

        public UserAssessmentsController(IUserAssessmentService userAssessmentService)
        {
            _userAssessmentService = userAssessmentService;
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin,Evaluator")]
        public async Task<IActionResult> AssignAssessmentToUsers([FromBody] UserAssessmentAssignDto assignDto)
        {
            await _userAssessmentService.AssignAssessmentToUsersAsync(assignDto);
            return Ok(new { message = "Assessments assigned successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAssessments()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserRole == "Admin" || currentUserRole == "Evaluator")
            {
                var assessments = await _userAssessmentService.GetUserAssessmentsForAdminAsync();
                return Ok(assessments);
            }
            else
            {
                var assessments = await _userAssessmentService.GetUserAssessmentsForUserAsync(currentUserId);
                return Ok(assessments);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAssessmentDetails(int id)
        {
            var userAssessment = await _userAssessmentService.GetUserAssessmentDetailsAsync(id);
            if (userAssessment == null)
            {
                return NotFound();
            }
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            if (userAssessment.UserId != currentUserId && currentUserRole != "Admin" && currentUserRole != "Evaluator")
            {
                return Forbid();
            }

            return Ok(userAssessment);
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartAssessment(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _userAssessmentService.StartAssessmentAsync(id, currentUserId);
                return Ok(new { message = "Assessment started." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitAssessment(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _userAssessmentService.SubmitAssessmentAsync(id, currentUserId);
                return Ok(new { message = "Assessment submitted and result generation started." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
