//Purpose: For a candidate to submit an answer to a specific question during an assessment
using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class SubmissionCreateDto
    {
        [Required(ErrorMessage = "UserAssessment ID is required.")]
        public int UserAssessmentId { get; set; }

        [Required(ErrorMessage = "Question ID is required.")]
        public int QuestionId { get; set; }

        // Answer text for Descriptive/MCQ. Only one of these will likely be populated.
        public string? AnswerText { get; set; }

        // Path to uploaded file for FileUpload questions.
        public string? AnswerFilePath { get; set; }
    }

}
