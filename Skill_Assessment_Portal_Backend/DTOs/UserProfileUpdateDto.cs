using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class UserProfileUpdateDto
    {
        [StringLength(100)]
        public string? FullName { get; set; } 

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; } 

        [StringLength(500)]
        public string? ProfilePicturePath { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(100)]
        public string? HighestQualification { get; set; }

        public bool? IsEmployed { get; set; }

        [StringLength(100)]
        public string? CurrentRole { get; set; }
    }
}
