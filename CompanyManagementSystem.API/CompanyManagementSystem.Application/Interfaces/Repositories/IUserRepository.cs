using CompanyManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByUserNameAsync(string userName);

        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsByUserNameAsync(string userName);

        Task<bool> IsUserBannedAsync(string Email);

        Task<bool> IsUserEmailVerfiedAsync(string Email);

        Task<Guid> GetUserIdFromUserName(string userName);

        // CreateAsync uses UserManager internally — sets NormalizedEmail, NormalizedUserName,
        // SecurityStamp, and hashes the password all in one call.
        // Returns IdentityResult so the caller can inspect errors (duplicate email, weak password, etc.)
        Task<IEnumerable<string>> CreateAsync(User user, string password);

        // UpdateAsync uses UserManager internally — keeps SecurityStamp in sync.
        Task UpdateAsync(User user);

        // UpdatePasswordAsync uses UserManager.RemovePasswordAsync + AddPasswordAsync
        // so Identity updates SecurityStamp and rehashes correctly.
        Task UpdatePasswordAsync(User user, string newPassword);

        Task DeleteAsync(User user);

        // AddAsync + SaveChangesAsync kept but commented out in implementation —
        // preserved so nothing else in the codebase breaks.
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
