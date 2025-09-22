//Purpose: Specific data access operations for Submission entities.
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Interfaces.IRepositories
{
    public interface ISubmissionRepository : IGenericRepository<Submission>
    {
        Task<IEnumerable<Submission>> GetSubmissionsByUserAssessmentIdAsync(int userAssessmentId);
        Task<Submission> GetSubmissionWithDetailsAsync(int submissionId); // Include Question
    }
}
