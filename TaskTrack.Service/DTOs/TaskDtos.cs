using System.ComponentModel.DataAnnotations;
using TaskTrack.Repo.Models;

namespace TaskTrack.Service.DTOs
{
    // ─── Read ────────────────────────────────────────────────────────────────────
    public class TaskReadDto
    {
        public int TaskID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatusEnum Status { get; set; }
        public string StatusLabel => Status.ToString();
        public TaskPriority Priority { get; set; }
        public string PriorityLabel => Priority.ToString();
        public DateOnly? DueDate { get; set; }
        public int ProjectID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class TaskDetailDto : TaskReadDto
    {
        public List<TagReadDto> Tags { get; set; } = new();
    }

    // ─── Create ──────────────────────────────────────────────────────────────────
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Task title is required.")]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateOnly? DueDate { get; set; }

        [Required(ErrorMessage = "ProjectID is required.")]
        public int ProjectID { get; set; }

        public List<int>? TagIDs { get; set; }
    }

    // ─── Update ──────────────────────────────────────────────────────────────────
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Task title is required.")]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public TaskStatusEnum Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateOnly? DueDate { get; set; }

        [Required(ErrorMessage = "ProjectID is required.")]
        public int ProjectID { get; set; }

        public List<int>? TagIDs { get; set; }
    }
}
