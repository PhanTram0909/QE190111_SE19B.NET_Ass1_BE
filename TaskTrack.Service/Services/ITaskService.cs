using TaskTrack.Repo.Models;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskReadDto>> GetAllActiveAsync();
        Task<TaskDetailDto?> GetByIdAsync(int id);
        Task<IEnumerable<TaskReadDto>> GetByProjectAsync(int projectId);
        Task<TaskDetailDto> CreateAsync(TaskCreateDto dto, int? createdById = null);
        Task<TaskDetailDto?> UpdateAsync(int id, TaskUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TaskDetailDto>> SearchAsync(string? title, TaskStatusEnum? status, TaskPriority? priority, int? projectId, int? tagId);
    }
}
