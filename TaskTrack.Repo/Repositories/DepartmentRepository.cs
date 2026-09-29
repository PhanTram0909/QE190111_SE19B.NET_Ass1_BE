using Microsoft.EntityFrameworkCore;
using TaskTrack.Repo.Data;
using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(TaskTrackDbContext context) : base(context) { }

        public async Task<IEnumerable<Department>> GetAllActiveAsync()
        {
            return await _context.Departments
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<Department?> GetWithProjectsAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Projects.Where(p => p.IsActive))
                .FirstOrDefaultAsync(d => d.DepartmentID == id);
        }

        public async Task<IEnumerable<Department>> SearchByNameAsync(string name)
        {
            return await _context.Departments
                .Where(d => d.IsActive && d.DepartmentName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<bool> HasProjectsAsync(int departmentId)
        {
            return await _context.Projects
                .AnyAsync(p => p.DepartmentID == departmentId && p.IsActive);
        }
    }
}
