//Purpose: Specific data access operations for UserAssessment entities.

using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Interfaces.IRepositories
{
    public interface IUserAssessmentRepository : IGenericRepository<UserAssessment>
    {
        Task<IEnumerable<UserAssessment>> GetUserAssessmentsByUserIdAsync(int userId);
        Task<IEnumerable<UserAssessment>> GetUserAssessmentsByAssessmentIdAsync(int assessmentId);
        Task<UserAssessment> GetUserAssessmentWithDetailsAsync(int userAssessmentId); // Include User, Assessment, Result, Submissions
    }
}
