//Purpose: For adding a new question to an assessment

using Skill_Assessment_Portal_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class QuestionCreateDto
    {
        // AssessmentId will come from the route or parent context, not directly in this DTO.

        [Required(ErrorMessage = "Question text is required.")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Question type is required.")]
        public QuestionType QuestionType { get; set; }

        // For MCQ, this will be a JSON string of options (e.g., ["Option A", "Option B"])
        public List<string>? Options { get; set; }

        // For MCQ, this would be the correct option; for others, it's a model answer/expected output.
        [Required(ErrorMessage = "Correct answer is required.")]
        public string CorrectAnswer { get; set; }

        [Required(ErrorMessage = "Difficulty level is required.")]
        public DifficultyLevel DifficultyLevel { get; set; }

        [Required(ErrorMessage = "Max marks for the question are required.")]
        [Range(1, 100, ErrorMessage = "Max marks must be between 1 and 100.")]
        public int MaxMarks { get; set; }

        public string? ReferenceFilePath { get; set; } // Path to an image, code snippet, etc.
    }
}


