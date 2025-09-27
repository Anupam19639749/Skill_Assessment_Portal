using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = null!; 
        [Required] 
        public byte[] PasswordSalt { get; set; } = null!;

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(100)]
        public string? HighestQualification { get; set; }

        public bool IsEmployed { get; set; } = false;

        [StringLength(100)]
        public string? CurrentRole { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public string? ProfilePicturePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Assessment>? CreatedAssessments { get; set; }
        public ICollection<UserAssessment>? UserAssessments { get; set; }
    }
}
