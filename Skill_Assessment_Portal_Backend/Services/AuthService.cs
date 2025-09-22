using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Skill_Assessment_Portal_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration; // For accessing JWT settings

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            if (await _userRepository.UserExistsAsync(userRegisterDto.Email))
            {
                throw new ApplicationException("User with this email already exists.");
            }

            // New: Use the corrected hashing method
            CreatePasswordHash(userRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = _mapper.Map<User>(userRegisterDto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt; // Store the new salt

            var role = await _roleRepository.GetByIdAsync(userRegisterDto.RoleId);
            if (role == null)
            {
                role = await _roleRepository.GetRoleByNameAsync("Candidate");
                user.RoleId = role.RoleId;
            }

            await _userRepository.AddAsync(user);

            var registeredUser = await _userRepository.GetUserByEmailAsync(user.Email);
            return GenerateAuthResponse(registeredUser);
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email);

            // New: Use the corrected verification method
            if (user == null || !VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return GenerateAuthResponse(user);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            // Include role for mapping
            var userWithRole = await _userRepository.GetUserByEmailAsync(user.Email); // Re-fetch to include role
            return _mapper.Map<UserDto>(userWithRole);
        }

        // --- Helper Methods for Password Hashing and JWT Generation ---
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private AuthResponseDto GenerateAuthResponse(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:DurationHours"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.RoleName,
                Token = tokenHandler.WriteToken(token),
                ExpiresOn = tokenDescriptor.Expires.Value
            };
        }
    }
}
