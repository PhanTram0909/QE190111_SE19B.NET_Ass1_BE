namespace TaskTrack.Repo.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentDescription { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
