using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public interface IAuthService
    {
        Task<AccountReadDto> RegisterStaffAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
