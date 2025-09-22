using AutoMapper;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            // Eager load roles for mapping
            var usersWithRoles = new List<User>();
            foreach (var user in users)
            {
                var userWithRole = await _userRepository.GetUserByEmailAsync(user.Email);
                if (userWithRole != null) usersWithRoles.Add(userWithRole);
            }
            return _mapper.Map<IEnumerable<UserDto>>(usersWithRoles);
        }

        public async Task<UserDto> GetUserDtoByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;
            var userWithRole = await _userRepository.GetUserByEmailAsync(user.Email); // Eager load role
            return _mapper.Map<UserDto>(userWithRole);
        }

        public async Task UpdateUserRoleAsync(int userId, int newRoleId)
        {
            var userToUpdate = await _userRepository.GetByIdAsync(userId);
            if (userToUpdate == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var newRole = await _roleRepository.GetByIdAsync(newRoleId);
            if (newRole == null)
            {
                throw new KeyNotFoundException($"Role with ID {newRoleId} not found.");
            }

            userToUpdate.RoleId = newRoleId;
            userToUpdate.UpdatedAt = DateTime.Now;
            _userRepository.Update(userToUpdate);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var userToDelete = await _userRepository.GetByIdAsync(userId);
            if (userToDelete == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            _userRepository.Delete(userToDelete);
        }
    }
}
