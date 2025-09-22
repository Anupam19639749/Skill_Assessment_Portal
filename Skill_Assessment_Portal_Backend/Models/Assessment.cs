using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skill_Assessment_Portal_Backend.Models
{
    public class Assessment
    {
        [Key]
        public int AssessmentId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(1, 1000)]
        public int DurationMinutes { get; set; }
        [ForeignKey("User")]

        [Required]
        public int CreatedBy { get; set; }
        public User Creator { get; set; } = null!;

        [StringLength(500)]
        public string? InstructionsFilePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Question>? Questions { get; set; }
        public ICollection<UserAssessment>? UserAssessments { get; set; }
    }
}
