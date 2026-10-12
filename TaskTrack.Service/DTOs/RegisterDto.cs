using System.ComponentModel.DataAnnotations;

namespace TaskTrack.Service.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(200, ErrorMessage = "Full Name cannot exceed 200 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
