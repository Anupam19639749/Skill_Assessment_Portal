//Purpose: Specific data access operations for Assessment entities.

using Skill_Assessment_Portal_Backend.Models;
namespace Skill_Assessment_Portal_Backend.Interfaces.IRepositories
{
    public interface IAssessmentRepository : IGenericRepository<Assessment>
    {
        Task<IEnumerable<Assessment>> GetAssessmentsWithDetailsAsync(); // E.g., include creator and question count
        Task<Assessment> GetAssessmentByIdWithDetailsAsync(int id); // Include related data for a single assessment
    }
}
