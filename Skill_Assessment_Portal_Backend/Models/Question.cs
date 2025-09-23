using Skill_Assessment_Portal_Backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skill_Assessment_Portal_Backend.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        [ForeignKey("Assessment")]
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; } = null!;

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public QuestionType QuestionType { get; set; }
        public List<string>? Options { get; set; }

        [Required]
        public string? CorrectAnswer { get; set; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxMarks { get; set; }

        [StringLength(500)]
        public string? ReferenceFilePath { get; set; }
        public ICollection<Submission>? Submissions { get; set; }
    }

}
