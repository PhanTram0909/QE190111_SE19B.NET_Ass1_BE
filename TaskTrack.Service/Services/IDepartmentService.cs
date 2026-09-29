using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentReadDto>> GetAllActiveAsync();
        Task<DepartmentWithProjectsDto?> GetByIdAsync(int id);
        Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto dto);
        Task<DepartmentReadDto?> UpdateAsync(int id, DepartmentUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DepartmentReadDto>> SearchByNameAsync(string name);
    }
}
