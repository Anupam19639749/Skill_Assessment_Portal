using Microsoft.EntityFrameworkCore;
using Skill_Assessment_Portal_Backend.Data;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Repositories
{
    public class UserAssessmentRepository : GenericRepository<UserAssessment>, IUserAssessmentRepository
    {
        public UserAssessmentRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<UserAssessment>> GetUserAssessmentsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(ua => ua.Assessment)
                .Where(ua => ua.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserAssessment>> GetUserAssessmentsByAssessmentIdAsync(int assessmentId)
        {
            return await _dbSet
                .Include(ua => ua.User).ThenInclude(u => u.Role)
                .Where(ua => ua.AssessmentId == assessmentId)
                .ToListAsync();
        }

        public async Task<UserAssessment> GetUserAssessmentWithDetailsAsync(int userAssessmentId)
        {
            return await _dbSet
                .Include(ua => ua.User).ThenInclude(u => u.Role)
                .Include(ua => ua.Assessment).ThenInclude(a => a.Creator)
                .Include(ua => ua.Result)
                .SingleOrDefaultAsync(ua => ua.UserAssessmentId == userAssessmentId);
        }
    }

}
