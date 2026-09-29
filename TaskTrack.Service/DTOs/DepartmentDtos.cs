using System.ComponentModel.DataAnnotations;

namespace TaskTrack.Service.DTOs
{
    // ─── Read ────────────────────────────────────────────────────────────────────
    public class DepartmentReadDto
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentDescription { get; set; }
        public bool IsActive { get; set; }
    }

    public class DepartmentWithProjectsDto : DepartmentReadDto
    {
        public List<ProjectReadDto> Projects { get; set; } = new();
    }

    // ─── Create ──────────────────────────────────────────────────────────────────
    public class DepartmentCreateDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(200)]
        public string DepartmentName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? DepartmentDescription { get; set; }
    }

    // ─── Update ──────────────────────────────────────────────────────────────────
    public class DepartmentUpdateDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(200)]
        public string DepartmentName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? DepartmentDescription { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
