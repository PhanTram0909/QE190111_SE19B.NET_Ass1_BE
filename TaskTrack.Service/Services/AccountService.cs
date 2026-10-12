using TaskTrack.Repo.Models;
using TaskTrack.Repo.UnitOfWork;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;

        public AccountService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<AccountReadDto>> GetAllAsync()
        {
            var accounts = await _uow.Accounts.GetAllAsync();
            return accounts.Select(MapToReadDto);
        }

        public async Task<AccountReadDto?> GetByIdAsync(int id)
        {
            var account = await _uow.Accounts.GetByIdAsync(id);
            if (account == null) return null;
            return MapToReadDto(account);
        }

        public async Task<AccountReadDto?> UpdateAsync(int id, AccountUpdateDto dto)
        {
            var account = await _uow.Accounts.GetByIdAsync(id);
            if (account == null) return null;

            account.FullName = dto.FullName.Trim();
            account.Role = dto.Role;

            _uow.Accounts.Update(account);
            await _uow.SaveChangesAsync();

            return MapToReadDto(account);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await _uow.Accounts.GetByIdAsync(id);
            if (account == null) return false;

            // Reject deletion if account has created any tasks (Assignment 2 Requirement)
            bool hasTasks = await _uow.Accounts.HasCreatedTasksAsync(id);
            if (hasTasks)
            {
                throw new InvalidOperationException("Cannot delete account because it has created tasks in the system. Reassign or remove all tasks first.");
            }

            _uow.Accounts.Delete(account);
            await _uow.SaveChangesAsync();
            return true;
        }

        private static AccountReadDto MapToReadDto(SystemAccount a) => new()
        {
            AccountID = a.AccountID,
            FullName = a.FullName,
            Email = a.Email,
            Role = a.Role,
            CreatedDate = a.CreatedDate
        };
    }
}
