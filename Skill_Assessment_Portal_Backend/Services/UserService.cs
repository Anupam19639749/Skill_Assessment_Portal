using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;
using System.Security.Cryptography;


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

        public async Task UpdateUserProfileAsync(int userId, UserProfileUpdateDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Apply updates only if the DTO property is not null
            if (!string.IsNullOrEmpty(updateDto.FullName))
            {
                user.FullName = updateDto.FullName;
            }
            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                user.Email = updateDto.Email;
            }
            if (!string.IsNullOrEmpty(updateDto.ProfilePicturePath))
            {
                user.ProfilePicturePath = updateDto.ProfilePicturePath;
            }
            if (!string.IsNullOrEmpty(updateDto.Gender))
            {
                user.Gender = updateDto.Gender;
            }
            if (!string.IsNullOrEmpty(updateDto.HighestQualification))
            {
                user.HighestQualification = updateDto.HighestQualification;
            }
            if (updateDto.IsEmployed.HasValue) // Check for nullable bool
            {
                user.IsEmployed = updateDto.IsEmployed.Value;
            }
            if (!string.IsNullOrEmpty(updateDto.CurrentRole))
            {
                user.CurrentRole = updateDto.CurrentRole;
            }

            user.UpdatedAt = DateTime.UtcNow;
            _userRepository.Update(user); 
        }

        // ChangeUserPasswordAsync
        public async Task ChangeUserPasswordAsync(int userId, UserPasswordUpdateDto passwordDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Verify current password
            if (!VerifyPasswordHash(passwordDto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Current password is incorrect.");
            }

            // Hash and update new password
            CreatePasswordHash(passwordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user); 
        }

        // --- Helper methods for password hashing
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
