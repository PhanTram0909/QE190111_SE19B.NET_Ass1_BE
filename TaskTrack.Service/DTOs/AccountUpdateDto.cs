using System.ComponentModel.DataAnnotations;

namespace TaskTrack.Service.DTOs
{
    public class AccountUpdateDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(200, ErrorMessage = "Full Name cannot exceed 200 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Range(0, 1, ErrorMessage = "Role must be either 0 (Staff) or 1 (Admin).")]
        public int Role { get; set; }
    }
}
