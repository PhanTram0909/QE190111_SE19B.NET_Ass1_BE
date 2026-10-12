namespace TaskTrack.Service.DTOs
{
    public class AccountReadDto
    {
        public int AccountID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }
        public string RoleLabel => Role == 1 ? "Admin" : "Staff";
        public DateTime CreatedDate { get; set; }
    }
}
