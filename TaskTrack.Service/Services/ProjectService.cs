using TaskTrack.Repo.Models;
using TaskTrack.Repo.UnitOfWork;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _uow;

        public ProjectService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<ProjectReadDto>> GetAllAsync()
        {
            var projects = await _uow.Projects.GetAllWithDepartmentAsync();
            return projects.Select(MapToReadDto);
        }

        public async Task<ProjectDetailDto?> GetByIdAsync(int id)
        {
            var project = await _uow.Projects.GetWithTasksAsync(id);
            if (project == null) return null;

            return new ProjectDetailDto
            {
                ProjectID = project.ProjectID,
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                DepartmentID = project.DepartmentID,
                DepartmentName = project.Department?.DepartmentName,
                IsActive = project.IsActive,
                CreatedDate = project.CreatedDate,
                Tasks = project.Tasks.Select(t => new TaskReadDto
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
                }).ToList()
            };
        }

        public async Task<IEnumerable<ProjectReadDto>> GetByDepartmentAsync(int departmentId)
        {
            var projects = await _uow.Projects.GetByDepartmentAsync(departmentId);
            return projects.Select(MapToReadDto);
        }

        public async Task<ProjectReadDto> CreateAsync(ProjectCreateDto dto)
        {
            var entity = new Project
            {
                ProjectName = dto.ProjectName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                DepartmentID = dto.DepartmentID,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _uow.Projects.AddAsync(entity);
            await _uow.SaveChangesAsync();

            // reload with department name
            var created = await _uow.Projects.GetWithTasksAsync(entity.ProjectID);
            return MapToReadDto(created!);
        }

        public async Task<ProjectReadDto?> UpdateAsync(int id, ProjectUpdateDto dto)
        {
            var entity = await _uow.Projects.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ProjectName = dto.ProjectName;
            entity.Description = dto.Description;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.Status = dto.Status;
            entity.DepartmentID = dto.DepartmentID;
            entity.IsActive = dto.IsActive;

            _uow.Projects.Update(entity);
            await _uow.SaveChangesAsync();

            var updated = await _uow.Projects.GetWithTasksAsync(entity.ProjectID);
            return MapToReadDto(updated!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _uow.Projects.GetByIdAsync(id);
            if (entity == null) return false;

            var hasTasks = await _uow.Projects.HasTasksAsync(id);
            if (hasTasks)
                throw new InvalidOperationException(
                    "Cannot delete project because it has linked active tasks. Please remove or reassign all tasks first.");

            _uow.Projects.Delete(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProjectReadDto>> SearchAsync(string? name, ProjectStatus? status, int? departmentId)
        {
            var results = await _uow.Projects.SearchAsync(name, status, departmentId);
            return results.Select(MapToReadDto);
        }

        // ─── Mapping helper ───────────────────────────────────────────────────────
        private static ProjectReadDto MapToReadDto(Project p) => new()
        {
            ProjectID = p.ProjectID,
            ProjectName = p.ProjectName,
            Description = p.Description,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Status = p.Status,
            DepartmentID = p.DepartmentID,
            DepartmentName = p.Department?.DepartmentName,
            IsActive = p.IsActive,
            CreatedDate = p.CreatedDate
        };
    }
}
