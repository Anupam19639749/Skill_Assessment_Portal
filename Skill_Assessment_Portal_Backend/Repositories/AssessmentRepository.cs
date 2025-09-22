using Microsoft.EntityFrameworkCore;
using Skill_Assessment_Portal_Backend.Data;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Repositories
{
    public class AssessmentRepository : GenericRepository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<Assessment>> GetAssessmentsWithDetailsAsync()
        {
            return await _dbSet
                .Include(a => a.Creator)
                .Include(a => a.Questions) // Eager load questions to get a count
                .ToListAsync();
        }

        public async Task<Assessment> GetAssessmentByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Creator)
                .Include(a => a.Questions)
                .SingleOrDefaultAsync(a => a.AssessmentId == id);
        }
    }

}
