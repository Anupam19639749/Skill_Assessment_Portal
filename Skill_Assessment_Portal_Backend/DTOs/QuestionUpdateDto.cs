//Purpose: For updating an existing question. All fields are nullable to allow partial updates

using Skill_Assessment_Portal_Backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class QuestionUpdateDto
    {
        public string? QuestionText { get; set; }
        public QuestionType? QuestionType { get; set; }
        public List<string>? Options { get; set; }
        public string? CorrectAnswer { get; set; }
        public DifficultyLevel? DifficultyLevel { get; set; }

        [Range(1, 100, ErrorMessage = "Max marks must be between 1 and 100.")]
        public int? MaxMarks { get; set; }
        public string? ReferenceFilePath { get; set; }
    }
}
