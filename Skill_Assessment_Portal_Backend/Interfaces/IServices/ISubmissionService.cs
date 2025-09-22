//Purpose: Business logic for handling candidate submissions
using Skill_Assessment_Portal_Backend.DTOs;

namespace Skill_Assessment_Portal_Backend.Interfaces.IServices
{
    public interface ISubmissionService
    {
        Task<SubmissionDto> CreateSubmissionAsync(SubmissionCreateDto submissionDto, int userId); // userId for validation
        Task<IEnumerable<SubmissionDto>> GetSubmissionsByUserAssessmentIdAsync(int userAssessmentId, int userId); // userId for auth check
        Task<SubmissionDto> GetSubmissionByIdAsync(int submissionId, int userId); // userId for auth check
        // Potentially an update method for evaluator to correct/review submission?
    }
}
