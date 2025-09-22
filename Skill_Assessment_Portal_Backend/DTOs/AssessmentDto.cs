//Purpose: To retrieve and display assessment details
namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class AssessmentDto
    {
        public int AssessmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public int CreatedBy { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public string? InstructionsFilePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuestionCount { get; set; } // Useful for display
    }
}
