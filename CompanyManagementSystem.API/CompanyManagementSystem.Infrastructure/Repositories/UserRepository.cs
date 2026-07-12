using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ── Lookups ────────────────────────────────────────────────────────────────

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpperInvariant());
        }

        public async Task<Guid> GetUserIdFromUserName(string userName)
        {
            return await _context.Users
                .Where(u => u.NormalizedUserName == userName.ToUpperInvariant())
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
        }

        // ── Existence checks ───────────────────────────────────────────────────────

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
        }

        public async Task<bool> ExistsByUserNameAsync(string userName)
        {
            return await _context.Users
                .AnyAsync(u => u.NormalizedUserName == userName.ToUpperInvariant());
        }

        public async Task<bool> IsUserBannedAsync(string email)
        {
            return await _context.Users
                .Where(u => u.NormalizedEmail == email.ToUpperInvariant())
                .Select(u => u.IsBanned)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserEmailVerfiedAsync(string email)
        {
            return await _context.Users
                .Where(u => u.NormalizedEmail == email.ToUpperInvariant())
                .Select(u => u.EmailConfirmed)
                .FirstOrDefaultAsync();
        }

        // ── Write operations ───────────────────────────────────────────────────────

        /// <summary>
        /// Creates the user through Identity's pipeline.
        /// Sets NormalizedEmail, NormalizedUserName, SecurityStamp, and hashes the password.
        /// Returns a list of error messages — empty list means success.
        /// </summary>
        public async Task<IEnumerable<string>> CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                return Enumerable.Empty<string>();

            return result.Errors.Select(e => e.Description);
        }

        /// <summary>
        /// Updates user profile fields (EmailConfirmed, IsBanned, Role, etc.)
        /// through UserManager so SecurityStamp stays in sync.
        /// </summary>
        public async Task UpdateAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Resets the user's password through Identity's pipeline.
        /// UserManager removes the old hash, rehashes the new password,
        /// and rotates the SecurityStamp — all in one call.
        /// </summary>
        public async Task UpdatePasswordAsync(User user, string newPassword)
        {
            // Remove existing password then set the new one.
            // This is the correct Identity way to reset a password without a token.
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);
        }

        public async Task DeleteAsync(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        // ── Legacy methods kept for compatibility ──────────────────────────────────
        // These are no longer the primary path but kept so nothing breaks.

        public async Task AddAsync(User user)
        {
            // LEGACY: Direct EF insert — does NOT set NormalizedEmail, NormalizedUserName,
            // SecurityStamp. Use CreateAsync(user, password) instead for registration.
            // Kept here so the interface contract is satisfied without breaking callers.
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
