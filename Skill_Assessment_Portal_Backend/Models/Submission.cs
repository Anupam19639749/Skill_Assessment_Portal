using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skill_Assessment_Portal_Backend.Models
{
    public class Submission
    {
        [Key]
        public int SubmissionId { get; set; }

        [Required]
        [ForeignKey("UserAssessment")]
        public int UserAssessmentId { get; set; }
        public UserAssessment UserAssessment { get; set; } = null!;

        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public string? AnswerText { get; set; }

        [StringLength(500)]
        public string? AnswerFilePath { get; set; }

        public bool? IsCorrect { get; set; }

        public int? MarksObtained { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }

}
