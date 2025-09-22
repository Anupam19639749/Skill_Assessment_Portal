//Purpose: Specific data access operations for User entities

using Skill_Assessment_Portal_Backend.Models;
namespace Skill_Assessment_Portal_Backend.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
    }
}
