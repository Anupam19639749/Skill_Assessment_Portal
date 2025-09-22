//Purpose: For an existing user to log in.
using System.ComponentModel.DataAnnotations;

namespace Skill_Assessment_Portal_Backend.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
