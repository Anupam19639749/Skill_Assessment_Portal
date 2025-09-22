using Microsoft.EntityFrameworkCore;
using Skill_Assessment_Portal_Backend.Data;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<Question>> GetQuestionsByAssessmentIdAsync(int assessmentId)
        {
            return await _dbSet.Where(q => q.AssessmentId == assessmentId).ToListAsync();
        }
    }
}
