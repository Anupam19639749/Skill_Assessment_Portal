//Purpose: To display the final results of a completed assessment
namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class ResultDto
    {
        public int ResultId { get; set; }
        public int UserAssessmentId { get; set; }
        public int UserId { get; set; } // From UserAssessment
        public string UserName { get; set; } // From UserAssessment
        public int AssessmentId { get; set; } // From UserAssessment
        public string AssessmentTitle { get; set; } // From UserAssessment
        public int TotalMarksObtained { get; set; }
        public float Percentage { get; set; }
        public bool Passed { get; set; }
        public DateTime EvaluatedAt { get; set; }
    }
}
