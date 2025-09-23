//Purpose: To retrieve and display question details

using Skill_Assessment_Portal_Backend.Enums;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public int AssessmentId { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public List<string>? Options { get; set; }
        public string CorrectAnswer { get; set; } // Might be omitted for candidates
        public DifficultyLevel DifficultyLevel { get; set; }
        public int MaxMarks { get; set; }
        public string? ReferenceFilePath { get; set; }
    }
}

