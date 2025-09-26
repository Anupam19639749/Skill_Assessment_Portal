//Purpose: To display a candidate's assigned assessment status (e.g., on their dashboard).

using Skill_Assessment_Portal_Backend.Enums;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class UserAssessmentDto
    {
        public int UserAssessmentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } // For admin/evaluator views
        public int AssessmentId { get; set; }
        public string AssessmentTitle { get; set; }
        public UserAssessmentStatus Status { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; } // From the Assessment, for candidate info
        public int? TotalMarksObtained { get; set; } // From Result, if available
        public float? Percentage { get; set; } // From Result, if available
        public bool? Passed { get; set; } // From Result, if available
    }
}
