using TaskTrack.Repo.Models;
using TaskTrack.Repo.UnitOfWork;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _uow;

        public TaskService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<TaskReadDto>> GetAllActiveAsync()
        {
            var tasks = await _uow.Tasks.GetAllActiveAsync();
            return tasks.Select(MapToReadDto);
        }

        public async Task<TaskDetailDto?> GetByIdAsync(int id)
        {
            var task = await _uow.Tasks.GetWithTagsAsync(id);
            if (task == null) return null;
            return MapToDetailDto(task);
        }

        public async Task<IEnumerable<TaskReadDto>> GetByProjectAsync(int projectId)
        {
            var tasks = await _uow.Tasks.GetByProjectAsync(projectId);
            return tasks.Select(MapToReadDto);
        }

        public async Task<TaskDetailDto> CreateAsync(TaskCreateDto dto)
        {
            var entity = new Repo.Models.Task
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                ProjectID = dto.ProjectID,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            // Add TaskTags
            if (dto.TagIDs != null && dto.TagIDs.Any())
            {
                entity.TaskTags = dto.TagIDs.Select(tagId => new TaskTag
                {
                    TagID = tagId
                }).ToList();
            }

            await _uow.Tasks.AddAsync(entity);
            await _uow.SaveChangesAsync();

            // Reload with full tag navigation
            var created = await _uow.Tasks.GetWithTagsAsync(entity.TaskID);
            return MapToDetailDto(created!);
        }

        public async Task<TaskDetailDto?> UpdateAsync(int id, TaskUpdateDto dto)
        {
            var entity = await _uow.Tasks.GetWithTagsAsync(id);
            if (entity == null) return null;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Status = dto.Status;
            entity.Priority = dto.Priority;
            entity.DueDate = dto.DueDate;
            entity.ProjectID = dto.ProjectID;
            entity.ModifiedDate = DateTime.UtcNow;

            // Replace all tags entirely
            entity.TaskTags.Clear();
            if (dto.TagIDs != null && dto.TagIDs.Any())
            {
                foreach (var tagId in dto.TagIDs)
                {
                    entity.TaskTags.Add(new TaskTag { TaskID = id, TagID = tagId });
                }
            }

            _uow.Tasks.Update(entity);
            await _uow.SaveChangesAsync();

            var updated = await _uow.Tasks.GetWithTagsAsync(entity.TaskID);
            return MapToDetailDto(updated!);
        }

        /// <summary>Soft-delete: sets IsActive = false. Never hard-deletes.</summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _uow.Tasks.GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsActive = false;
            entity.ModifiedDate = DateTime.UtcNow;

            _uow.Tasks.Update(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskDetailDto>> SearchAsync(
            string? title,
            TaskStatusEnum? status,
            TaskPriority? priority,
            int? projectId,
            int? tagId)
        {
            var tasks = await _uow.Tasks.SearchAsync(title, status, priority, projectId, tagId);
            return tasks.Select(MapToDetailDto);
        }

        // ─── Mapping helpers ──────────────────────────────────────────────────────
        private static TaskReadDto MapToReadDto(Repo.Models.Task t) => new()
        {
            TaskID = t.TaskID,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            DueDate = t.DueDate,
            ProjectID = t.ProjectID,
            IsActive = t.IsActive,
            CreatedDate = t.CreatedDate,
            ModifiedDate = t.ModifiedDate
        };

        private static TaskDetailDto MapToDetailDto(Repo.Models.Task t) => new()
        {
            TaskID = t.TaskID,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            DueDate = t.DueDate,
            ProjectID = t.ProjectID,
            IsActive = t.IsActive,
            CreatedDate = t.CreatedDate,
            ModifiedDate = t.ModifiedDate,
            Tags = t.TaskTags?.Select(tt => new TagReadDto
            {
                TagID = tt.Tag.TagID,
                TagName = tt.Tag.TagName,
                Color = tt.Tag.Color
            }).ToList() ?? new List<TagReadDto>()
        };
    }
}
