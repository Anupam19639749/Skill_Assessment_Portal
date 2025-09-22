using Microsoft.EntityFrameworkCore;
using Skill_Assessment_Portal_Backend.Data;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDBContext context) : base(context) { }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _dbSet.SingleOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}
