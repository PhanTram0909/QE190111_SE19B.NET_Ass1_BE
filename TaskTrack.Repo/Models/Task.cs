namespace TaskTrack.Repo.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateOnly? DueDate { get; set; }
        public int ProjectID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        // Audit / Account link
        public int? CreatedByID { get; set; }
        public SystemAccount? CreatedBy { get; set; }

        // Navigation
        public Project Project { get; set; } = null!;
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
