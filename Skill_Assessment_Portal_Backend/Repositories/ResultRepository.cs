using Microsoft.EntityFrameworkCore;
using Skill_Assessment_Portal_Backend.Data;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Repositories
{
    public class ResultRepository : GenericRepository<Result>, IResultRepository
    {
        public ResultRepository(AppDBContext context) : base(context) { }

        public async Task<Result> GetResultByUserAssessmentIdAsync(int userAssessmentId)
        {
            return await _dbSet.SingleOrDefaultAsync(r => r.UserAssessmentId == userAssessmentId);
        }

        public async Task<IEnumerable<Result>> GetResultsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(r => r.UserAssessment).ThenInclude(ua => ua.Assessment)
                .Where(r => r.UserAssessment.UserId == userId)
                .ToListAsync();
        }
    }

}
