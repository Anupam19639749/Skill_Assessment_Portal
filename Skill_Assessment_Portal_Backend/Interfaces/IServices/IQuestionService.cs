//Purpose: Business logic for managing questions within an assessment.

using Skill_Assessment_Portal_Backend.DTOs;

namespace Skill_Assessment_Portal_Backend.Interfaces.IServices
{
    public interface IQuestionService
    {
        Task<QuestionDto> AddQuestionToAssessmentAsync(int assessmentId, QuestionCreateDto questionDto);
        Task<IEnumerable<QuestionDto>> GetQuestionsByAssessmentIdAsync(int assessmentId);
        Task<QuestionDto> GetQuestionByIdAsync(int questionId);
        Task UpdateQuestionAsync(int questionId, QuestionUpdateDto questionDto);
        Task DeleteQuestionAsync(int questionId);
    }
}
