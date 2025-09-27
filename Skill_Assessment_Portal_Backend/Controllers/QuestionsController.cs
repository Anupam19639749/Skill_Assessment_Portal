using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;

namespace Skill_Assessment_Portal_Backend.Controllers
{
    [ApiController]
    [Route("api/assessments/{assessmentId}/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Evaluator")]
        public async Task<IActionResult> AddQuestionToAssessment(int assessmentId, [FromBody] QuestionCreateDto questionDto)
        {
            try
            {
                var question = await _questionService.AddQuestionToAssessmentAsync(assessmentId, questionDto);
                return CreatedAtAction(nameof(GetQuestionById), new { assessmentId, questionId = question.QuestionId }, question);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetQuestionsByAssessmentId(int assessmentId)
        {
            var questions = await _questionService.GetQuestionsByAssessmentIdAsync(assessmentId);
            return Ok(questions);
        }

        [HttpGet("{questionId}")]
        [Authorize]
        public async Task<IActionResult> GetQuestionById(int assessmentId, int questionId)
        {
            var question = await _questionService.GetQuestionByIdAsync(questionId);
            if (question == null || question.AssessmentId != assessmentId)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPut("{questionId}")]
        [Authorize(Roles = "Admin,Evaluator")]
        public async Task<IActionResult> UpdateQuestion(int assessmentId, int questionId, [FromBody] QuestionUpdateDto questionDto)
        {
            try
            {
                var question = await _questionService.GetQuestionByIdAsync(questionId);
                if (question == null || question.AssessmentId != assessmentId)
                {
                    return NotFound();
                }
                await _questionService.UpdateQuestionAsync(questionId, questionDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{questionId}")]
        [Authorize(Roles = "Admin,Evaluator")]
        public async Task<IActionResult> DeleteQuestion(int assessmentId, int questionId)
        {
            try
            {
                var question = await _questionService.GetQuestionByIdAsync(questionId);
                if (question == null || question.AssessmentId != assessmentId)
                {
                    return NotFound();
                }
                await _questionService.DeleteQuestionAsync(questionId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
