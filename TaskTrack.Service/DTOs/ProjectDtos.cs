using System.ComponentModel.DataAnnotations;
using TaskTrack.Repo.Models;

namespace TaskTrack.Service.DTOs
{
    // ─── Read ────────────────────────────────────────────────────────────────────
    public class ProjectReadDto
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public string StatusLabel => Status.ToString();
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ProjectDetailDto : ProjectReadDto
    {
        public List<TaskReadDto> Tasks { get; set; } = new();
    }

    // ─── Create ──────────────────────────────────────────────────────────────────
    public class ProjectCreateDto
    {
        [Required(ErrorMessage = "Project name is required.")]
        [MaxLength(200)]
        public string ProjectName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;

        [Required(ErrorMessage = "DepartmentID is required.")]
        public int DepartmentID { get; set; }
    }

    // ─── Update ──────────────────────────────────────────────────────────────────
    public class ProjectUpdateDto
    {
        [Required(ErrorMessage = "Project name is required.")]
        [MaxLength(200)]
        public string ProjectName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public ProjectStatus Status { get; set; }

        [Required(ErrorMessage = "DepartmentID is required.")]
        public int DepartmentID { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
