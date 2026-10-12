using TaskTrack.Repo.Data;
using TaskTrack.Repo.Repositories;

namespace TaskTrack.Repo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskTrackDbContext _context;

        public IDepartmentRepository Departments { get; }
        public IProjectRepository Projects { get; }
        public ITaskRepository Tasks { get; }
        public ITagRepository Tags { get; }
        public IAccountRepository Accounts { get; }

        public UnitOfWork(TaskTrackDbContext context)
        {
            _context = context;
            Departments = new DepartmentRepository(context);
            Projects = new ProjectRepository(context);
            Tasks = new TaskRepository(context);
            Tags = new TagRepository(context);
            Accounts = new AccountRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
