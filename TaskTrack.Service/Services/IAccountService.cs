using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountReadDto>> GetAllAsync();
        Task<AccountReadDto?> GetByIdAsync(int id);
        Task<AccountReadDto?> UpdateAsync(int id, AccountUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
