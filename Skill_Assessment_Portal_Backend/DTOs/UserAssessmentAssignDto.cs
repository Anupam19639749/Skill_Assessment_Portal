//Purpose: For an admin to assign an assessment to one or more users
using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class UserAssessmentAssignDto
    {
        [Required(ErrorMessage = "Assessment ID is required.")]
        public int AssessmentId { get; set; }

        [Required(ErrorMessage = "At least one User ID is required.")]
        public List<int> UserIds { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
