using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<Guid> GetUserIdFromUserName(string userName)
        {
            return await _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsByUserNameAsync(string userName)
        {
            return await _context.Users
                .AnyAsync(x => x.UserName == userName);
        }

        public async Task<bool> IsUserBannedAsync(string Email)
        {
            return await _context.Users
                .Where(u => u.Email == Email)
                .Select(u => u.IsBanned)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserEmailVerfiedAsync(string Email)
        {
            return await _context.Users
                .Where(u => u.Email == Email)
                .Select(u => u.IsEmailVerfied)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
