using Microsoft.EntityFrameworkCore;
using TaskTrack.Repo.Data;
using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(TaskTrackDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> GetAllWithDepartmentAsync()
        {
            return await _context.Projects
                .Include(p => p.Department)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Project?> GetWithTasksAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Department)
                .Include(p => p.Tasks.Where(t => t.IsActive))
                .FirstOrDefaultAsync(p => p.ProjectID == id);
        }

        public async Task<IEnumerable<Project>> GetByDepartmentAsync(int departmentId)
        {
            return await _context.Projects
                .Include(p => p.Department)
                .Where(p => p.DepartmentID == departmentId && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> SearchAsync(string? name, ProjectStatus? status, int? departmentId)
        {
            var query = _context.Projects
                .Include(p => p.Department)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.ProjectName.ToLower().Contains(name.ToLower()));

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (departmentId.HasValue)
                query = query.Where(p => p.DepartmentID == departmentId.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> HasTasksAsync(int projectId)
        {
            return await _context.Tasks
                .AnyAsync(t => t.ProjectID == projectId && t.IsActive);
        }
    }
}
