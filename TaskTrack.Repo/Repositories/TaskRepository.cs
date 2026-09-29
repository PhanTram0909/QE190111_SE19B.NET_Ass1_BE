using Microsoft.EntityFrameworkCore;
using TaskTrack.Repo.Data;
using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public class TaskRepository : GenericRepository<Models.Task>, ITaskRepository
    {
        public TaskRepository(TaskTrackDbContext context) : base(context) { }

        public async Task<IEnumerable<Models.Task>> GetAllActiveAsync()
        {
            return await _context.Tasks
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<Models.Task?> GetWithTagsAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.TaskID == id);
        }

        public async Task<IEnumerable<Models.Task>> GetByProjectAsync(int projectId)
        {
            return await _context.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Where(t => t.ProjectID == projectId && t.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Task>> SearchAsync(
            string? title,
            TaskStatusEnum? status,
            TaskPriority? priority,
            int? projectId,
            int? tagId)
        {
            var query = _context.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Where(t => t.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(t => t.Title.ToLower().Contains(title.ToLower()));

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (projectId.HasValue)
                query = query.Where(t => t.ProjectID == projectId.Value);

            if (tagId.HasValue)
                query = query.Where(t => t.TaskTags.Any(tt => tt.TagID == tagId.Value));

            return await query.ToListAsync();
        }
    }
}
