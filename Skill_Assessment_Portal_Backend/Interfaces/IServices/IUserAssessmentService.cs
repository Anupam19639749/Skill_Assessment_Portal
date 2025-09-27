//Purpose: Business logic for assigning assessments to users and managing their attempts.
using Skill_Assessment_Portal_Backend.DTOs;

namespace Skill_Assessment_Portal_Backend.Interfaces.IServices
{
    public interface IUserAssessmentService
    {
        Task AssignAssessmentToUsersAsync(UserAssessmentAssignDto assignDto);
        Task<IEnumerable<UserAssessmentDto>> GetUserAssessmentsForUserAsync(int userId);
        Task<IEnumerable<UserAssessmentDto>> GetUserAssessmentsForAdminAsync(); // All user assessments for admin
        Task<UserAssessmentDto> GetUserAssessmentDetailsAsync(int userAssessmentId);
        Task<IEnumerable<UserAssessmentDto>> GetUserAssessmentsByAssessmentIdAsync(int assessmentId);
        Task StartAssessmentAsync(int userAssessmentId, int userId); // Marks status InProgress
        Task SubmitAssessmentAsync(int userAssessmentId, int userId); // Marks status Submitted, triggers auto-evaluation
        Task UnassignAssessmentAsync(int userAssessmentId);
    }
}
