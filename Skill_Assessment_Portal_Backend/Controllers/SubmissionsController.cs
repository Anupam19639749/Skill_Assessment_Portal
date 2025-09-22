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
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubmission([FromBody] SubmissionCreateDto submissionDto)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var submission = await _submissionService.CreateSubmissionAsync(submissionDto, currentUserId);
                return CreatedAtAction(nameof(GetSubmissionById), new { id = submission.SubmissionId }, submission);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("by-user-assessment/{userAssessmentId}")]
        public async Task<IActionResult> GetSubmissionsByUserAssessmentId(int userAssessmentId)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var submissions = await _submissionService.GetSubmissionsByUserAssessmentIdAsync(userAssessmentId, currentUserId);
                return Ok(submissions);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubmissionById(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var submission = await _submissionService.GetSubmissionByIdAsync(id, currentUserId);
                if (submission == null)
                {
                    return NotFound();
                }
                return Ok(submission);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
