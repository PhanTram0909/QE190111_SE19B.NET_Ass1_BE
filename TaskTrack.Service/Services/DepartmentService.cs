using TaskTrack.Repo.Models;
using TaskTrack.Repo.UnitOfWork;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _uow;

        public DepartmentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<DepartmentReadDto>> GetAllActiveAsync()
        {
            var departments = await _uow.Departments.GetAllActiveAsync();
            return departments.Select(MapToReadDto);
        }

        public async Task<DepartmentWithProjectsDto?> GetByIdAsync(int id)
        {
            var dept = await _uow.Departments.GetWithProjectsAsync(id);
            if (dept == null) return null;

            return new DepartmentWithProjectsDto
            {
                DepartmentID = dept.DepartmentID,
                DepartmentName = dept.DepartmentName,
                DepartmentDescription = dept.DepartmentDescription,
                IsActive = dept.IsActive,
                Projects = dept.Projects.Select(p => new ProjectReadDto
                {
                    ProjectID = p.ProjectID,
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                    DepartmentID = p.DepartmentID,
                    DepartmentName = dept.DepartmentName,
                    IsActive = p.IsActive,
                    CreatedDate = p.CreatedDate
                }).ToList()
            };
        }

        public async Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto dto)
        {
            var entity = new Department
            {
                DepartmentName = dto.DepartmentName,
                DepartmentDescription = dto.DepartmentDescription,
                IsActive = true
            };

            await _uow.Departments.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return MapToReadDto(entity);
        }

        public async Task<DepartmentReadDto?> UpdateAsync(int id, DepartmentUpdateDto dto)
        {
            var entity = await _uow.Departments.GetByIdAsync(id);
            if (entity == null) return null;

            entity.DepartmentName = dto.DepartmentName;
            entity.DepartmentDescription = dto.DepartmentDescription;
            entity.IsActive = dto.IsActive;

            _uow.Departments.Update(entity);
            await _uow.SaveChangesAsync();

            return MapToReadDto(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _uow.Departments.GetByIdAsync(id);
            if (entity == null) return false;

            var hasProjects = await _uow.Departments.HasProjectsAsync(id);
            if (hasProjects)
                throw new InvalidOperationException(
                    "Cannot delete department because it has linked active projects. Please remove or reassign all projects first.");

            _uow.Departments.Delete(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DepartmentReadDto>> SearchByNameAsync(string name)
        {
            var results = await _uow.Departments.SearchByNameAsync(name);
            return results.Select(MapToReadDto);
        }

        // ─── Mapping helper ───────────────────────────────────────────────────────
        private static DepartmentReadDto MapToReadDto(Department d) => new()
        {
            DepartmentID = d.DepartmentID,
            DepartmentName = d.DepartmentName,
            DepartmentDescription = d.DepartmentDescription,
            IsActive = d.IsActive
        };
    }
}
