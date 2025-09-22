//Purpose: To retrieve and display submission details (for evaluation or candidate review).
namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class SubmissionDto
    {
        public int SubmissionId { get; set; }
        public int UserAssessmentId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } // To display the question
        public string? AnswerText { get; set; }
        public string? AnswerFilePath { get; set; }
        public bool? IsCorrect { get; set; }
        public int? MarksObtained { get; set; }
        public int MaxMarks { get; set; } // From Question, for context
        public string CorrectAnswer { get; set; } // From Question, for evaluation/review
        public DateTime SubmittedAt { get; set; }
    }
}
