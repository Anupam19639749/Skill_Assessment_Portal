using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skill_Assessment_Portal_Backend.Models
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }

        [Required]
        [ForeignKey("UserAssessment")]
        public int UserAssessmentId { get; set; }
        public UserAssessment UserAssessment { get; set; } = null!;

        [Required]
        [Range(0, 9999)]
        public int TotalMarksObtained { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        public float Percentage { get; set; }

        [Required]
        public bool Passed { get; set; }
        public DateTime EvaluatedAt { get; set; } = DateTime.Now;
    }

}
