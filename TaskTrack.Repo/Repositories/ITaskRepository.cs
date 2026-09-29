using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public interface ITaskRepository : IGenericRepository<Models.Task>
    {
        Task<IEnumerable<Models.Task>> GetAllActiveAsync();
        Task<Models.Task?> GetWithTagsAsync(int id);
        Task<IEnumerable<Models.Task>> GetByProjectAsync(int projectId);
        Task<IEnumerable<Models.Task>> SearchAsync(string? title, TaskStatusEnum? status, TaskPriority? priority, int? projectId, int? tagId);
    }
}
