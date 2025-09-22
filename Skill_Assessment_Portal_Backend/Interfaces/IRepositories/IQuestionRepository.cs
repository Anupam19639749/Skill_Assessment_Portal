//Purpose: Specific data access operations for Question entities.

using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Interfaces.IRepositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsByAssessmentIdAsync(int assessmentId);
    }
}
