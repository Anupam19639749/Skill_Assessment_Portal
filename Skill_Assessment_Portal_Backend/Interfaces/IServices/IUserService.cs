//Purpose: Business logic for managing users (beyond authentication), typically by an Admin
using Skill_Assessment_Portal_Backend.DTOs;

namespace Skill_Assessment_Portal_Backend.Interfaces.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserDtoByIdAsync(int userId);
        Task UpdateUserRoleAsync(int userId, int newRoleId); // Admin functionality
        Task DeleteUserAsync(int userId); // Admin functionality

        Task UpdateUserProfileAsync(int userId, UserProfileUpdateDto updateDto);
        Task ChangeUserPasswordAsync(int userId, UserPasswordUpdateDto passwordDto);
    }
}
