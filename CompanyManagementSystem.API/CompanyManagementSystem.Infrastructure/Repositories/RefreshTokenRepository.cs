using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string Token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == Token);
        }

        public async Task CleanUsedToken(Guid id)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == id && rt.IsUsed)
                .ExecuteDeleteAsync();
        }

        public async Task CleanRevokedToken(Guid id)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == id && rt.IsRevoked)
                .ExecuteDeleteAsync();
        }

        public async Task CleanAllUsedTokens()
        {
            await _context.RefreshTokens
                .Where(rt => rt.IsUsed)
                .ExecuteDeleteAsync();
        }

        public async Task CleanAllRevokedTokens()
        {
            await _context.RefreshTokens
                .Where(rt => rt.IsRevoked)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteAllUserRefreshTokens(Guid id)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == id)
                .ExecuteDeleteAsync();
        }

        public async Task RevokeUserRefreshToken(Guid id)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == id && !rt.IsRevoked)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(rt => rt.IsRevoked, true));
        }

        public async Task<bool> UseRefreshToken(Guid id, string Token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt =>
                    rt.UserId == id &&
                    rt.Token == Token &&
                    !rt.IsRevoked &&
                    !rt.IsUsed &&
                    rt.ExpiresAt > DateTime.UtcNow);

            if (refreshToken is null)
            {
                return false;
            }

            refreshToken.IsUsed = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> CreateRefreshToken(Guid id, string Token)
        {
            var refreshToken = new RefreshToken
            {
                Token = Token,
                UserId = id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return Token;
        }
    }
}
