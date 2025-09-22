//Purpose: Business logic for user authentication and registration

using Skill_Assessment_Portal_Backend.DTOs;

namespace Skill_Assessment_Portal_Backend.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<AuthResponseDto> LoginAsync(UserLoginDto userLoginDto);
        Task<UserDto> GetUserByIdAsync(int userId);
    }
}
