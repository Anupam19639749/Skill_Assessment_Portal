//Purpose: For a new user to register in the system

using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        public string Password { get; set; }

        // RoleId will typically be set by the system (e.g., default to Candidate)
        // or by an Admin if an Admin is creating another Admin/Evaluator.
        // It's not usually exposed for direct registration unless there's a specific flow.
        public int RoleId { get; set; } = 2; // Default to Candidate (assuming RoleId 2 is Candidate)
    }

}
