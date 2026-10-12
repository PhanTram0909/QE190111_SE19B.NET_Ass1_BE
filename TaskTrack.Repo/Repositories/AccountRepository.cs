using Microsoft.EntityFrameworkCore;
using TaskTrack.Repo.Data;
using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Repositories
{
    public class AccountRepository : GenericRepository<SystemAccount>, IAccountRepository
    {
        public AccountRepository(TaskTrackDbContext context) : base(context) { }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.Email.ToLower() == email.Trim().ToLower());
        }

        public async Task<bool> HasCreatedTasksAsync(int accountId)
        {
            return await _context.Tasks
                .AnyAsync(t => t.CreatedByID == accountId);
        }
    }
}
