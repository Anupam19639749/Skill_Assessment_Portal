using Microsoft.EntityFrameworkCore;
using Skill_Assessment_Portal_Backend.Data;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Repositories
{
    public class SubmissionRepository : GenericRepository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<Submission>> GetSubmissionsByUserAssessmentIdAsync(int userAssessmentId)
        {
            return await _dbSet
                .Include(s => s.Question)
                .Where(s => s.UserAssessmentId == userAssessmentId)
                .ToListAsync();
        }

        public async Task<Submission> GetSubmissionWithDetailsAsync(int submissionId)
        {
            return await _dbSet
                .Include(s => s.Question)
                .SingleOrDefaultAsync(s => s.SubmissionId == submissionId);
        }
    }

}
