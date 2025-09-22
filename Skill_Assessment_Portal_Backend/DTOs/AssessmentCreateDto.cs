//Purpose: For creating a new assessment.

using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class AssessmentCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Duration in minutes is required.")]
        [Range(5, 240, ErrorMessage = "Duration must be between 5 and 240 minutes.")] // Example range
        public int DurationMinutes { get; set; }

        // CreatedBy will be taken from the JWT token, not sent by the client.
        // InstructionsFilePath will be handled separately (e.g., via file upload API)
    }
}
