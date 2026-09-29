using TaskTrack.Repo.Models;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectReadDto>> GetAllAsync();
        Task<ProjectDetailDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProjectReadDto>> GetByDepartmentAsync(int departmentId);
        Task<ProjectReadDto> CreateAsync(ProjectCreateDto dto);
        Task<ProjectReadDto?> UpdateAsync(int id, ProjectUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProjectReadDto>> SearchAsync(string? name, ProjectStatus? status, int? departmentId);
    }
}
