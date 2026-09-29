using TaskTrack.Repo.Repositories;

namespace TaskTrack.Repo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository Departments { get; }
        IProjectRepository Projects { get; }
        ITaskRepository Tasks { get; }
        ITagRepository Tags { get; }
        Task<int> SaveChangesAsync();
    }
}
