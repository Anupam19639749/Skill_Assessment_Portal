//Purpose: Business logic for managing assessments
using Skill_Assessment_Portal_Backend.DTOs;

namespace Skill_Assessment_Portal_Backend.Interfaces.IServices
{
    public interface IAssessmentService
    {
        Task<AssessmentDto> CreateAssessmentAsync(AssessmentCreateDto assessmentDto, int createdByUserId);
        Task<IEnumerable<AssessmentDto>> GetAllAssessmentsAsync();
        Task<AssessmentDto> GetAssessmentByIdAsync(int id);
        Task UpdateAssessmentAsync(int id, AssessmentUpdateDto assessmentDto);
        //Task DeleteAssessmentAsync(int id);
    }
}
