//Purpose: To display user information(e.g., in an admin panel)
using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; } // Display role name, not just ID
        public string? ProfilePicturePath { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(100)]
        public string? HighestQualification { get; set; }

        public bool? IsEmployed { get; set; }

        [StringLength(100)]
        public string? CurrentRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
