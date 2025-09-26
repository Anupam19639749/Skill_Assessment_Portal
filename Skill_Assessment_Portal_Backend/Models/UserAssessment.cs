using Skill_Assessment_Portal_Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skill_Assessment_Portal_Backend.Models
{
    public class UserAssessment
    {
        [Key]
        public int UserAssessmentId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        [ForeignKey("Assessment")]
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; } = null!;

        public DateTime ScheduledAt { get; set; }

        public UserAssessmentStatus Status { get; set; } = UserAssessmentStatus.NotStarted;

        public ICollection<Submission>? Submissions { get; set; }
        public Result? Result { get; set; }
    }
}
