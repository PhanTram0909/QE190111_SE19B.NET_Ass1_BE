using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetAllWithDepartmentAsync();
        Task<Project?> GetWithTasksAsync(int id);
        Task<IEnumerable<Project>> GetByDepartmentAsync(int departmentId);
        Task<IEnumerable<Project>> SearchAsync(string? name, ProjectStatus? status, int? departmentId);
        Task<bool> HasTasksAsync(int projectId);
    }
}
