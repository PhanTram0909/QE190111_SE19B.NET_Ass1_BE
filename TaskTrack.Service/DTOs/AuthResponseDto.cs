namespace TaskTrack.Service.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public AccountReadDto Account { get; set; } = null!;
    }
}
