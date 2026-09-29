namespace TaskTrack.Repo.Models
{
    public class Tag
    {
        public int TagID { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string? Color { get; set; }

        // Navigation
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
