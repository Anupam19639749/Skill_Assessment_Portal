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
    public class AssessmentsController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentsController(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Evaluator")] // Only Admins and Evaluators can create assessments
        public async Task<IActionResult> CreateAssessment([FromBody] AssessmentCreateDto assessmentDto)
        {
            var createdByUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var assessment = await _assessmentService.CreateAssessmentAsync(assessmentDto, createdByUserId);
            return CreatedAtAction(nameof(GetAssessmentById), new { id = assessment.AssessmentId }, assessment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssessments()
        {
            var assessments = await _assessmentService.GetAllAssessmentsAsync();
            return Ok(assessments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssessmentById(int id)
        {
            var assessment = await _assessmentService.GetAssessmentByIdAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }
            return Ok(assessment);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Evaluator")]
        public async Task<IActionResult> UpdateAssessment(int id, [FromBody] AssessmentUpdateDto assessmentDto)
        {
            try
            {
                await _assessmentService.UpdateAssessmentAsync(id, assessmentDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Evaluator")]
        //public async Task<IActionResult> DeleteAssessment(int id)
        //{
        //    try
        //    {
        //        await _assessmentService.DeleteAssessmentAsync(id);
        //        return NoContent();
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
