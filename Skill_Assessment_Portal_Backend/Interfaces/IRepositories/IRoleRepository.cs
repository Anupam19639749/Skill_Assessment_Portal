//Purpose: Specific data access operations for Role entities

using Skill_Assessment_Portal_Backend.Models;
namespace Skill_Assessment_Portal_Backend.Interfaces.IRepositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetRoleByNameAsync(string roleName);
    }
}
