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
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultsController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet("{userAssessmentId}")]
        public async Task<IActionResult> GetResultByUserAssessmentId(int userAssessmentId)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _resultService.GetResultByUserAssessmentIdAsync(userAssessmentId, currentUserId);
                if (result == null)
                {
                    return NotFound(new { message = "Result not found." });
                }
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPut("manual-evaluation/{submissionId}")]
        [Authorize(Roles = "Admin,Evaluator")]
        public async Task<IActionResult> UpdateResultManualEvaluation(int submissionId, [FromBody] int marksObtained)
        {
            try
            {
                await _resultService.UpdateResultManualEvaluationAsync(submissionId, marksObtained);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
