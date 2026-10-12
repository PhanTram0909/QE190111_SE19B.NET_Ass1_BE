using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public interface IAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<bool> HasCreatedTasksAsync(int accountId);
    }
}
