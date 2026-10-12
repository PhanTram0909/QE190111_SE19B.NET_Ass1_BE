namespace TaskTrack.Repo.Models
{
    public class SystemAccount
    {
        public int AccountID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int Role { get; set; } = 0; // 0 = Staff, 1 = Admin
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
    }
}
