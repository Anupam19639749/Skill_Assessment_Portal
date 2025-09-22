//Purpose: For updating an existing assessment. All fields are nullable to allow partial updates
using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class AssessmentUpdateDto
    {
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Range(5, 240, ErrorMessage = "Duration must be between 5 and 240 minutes.")]
        public int? DurationMinutes { get; set; }
    }
}
