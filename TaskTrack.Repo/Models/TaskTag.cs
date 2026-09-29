namespace TaskTrack.Repo.Models
{
    public class TaskTag
    {
        public int TaskID { get; set; }
        public int TagID { get; set; }

        // Navigation
        public Task Task { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
