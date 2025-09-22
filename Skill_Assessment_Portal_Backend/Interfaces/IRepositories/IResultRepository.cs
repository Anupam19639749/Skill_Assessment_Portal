//Purpose: Specific data access operations for Result entities

using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Interfaces.IRepositories
{
    public interface IResultRepository : IGenericRepository<Result>
    {
        Task<Result> GetResultByUserAssessmentIdAsync(int userAssessmentId);
        Task<IEnumerable<Result>> GetResultsByUserIdAsync(int userId);
    }
}
