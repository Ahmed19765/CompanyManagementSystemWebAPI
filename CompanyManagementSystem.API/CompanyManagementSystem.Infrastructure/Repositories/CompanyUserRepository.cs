using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class CompanyUserRepository : ICompanyUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public CompanyUserRepository(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;

        }

        public async Task<CompanyUser?> GetMembershipAsync(Guid companyId, Guid userId)
        {
            return await _context.CompanyUsers
                .FirstOrDefaultAsync(cu => cu.CompanyId == companyId && cu.UserId == userId);
        }

        public async Task<bool> IsMemberAsync(Guid companyId, Guid userId)
        {
            return await _context.CompanyUsers
                .AnyAsync(cu => cu.CompanyId == companyId && cu.UserId == userId);
        }

        public async Task AddAsync(CompanyUser companyUser)
        {
            await _context.CompanyUsers.AddAsync(companyUser);
        }

        public Task UpdateAsync(CompanyUser companyUser)
        {
            _context.CompanyUsers.Update(companyUser);
            return Task.CompletedTask;
        }

        public async Task RankUserUpAsLeader(string userName)
        {
            var userCom = await _context.CompanyUsers
                .Where(cu => cu.User!.NormalizedUserName == userName.ToUpperInvariant())
                .FirstOrDefaultAsync();

            if (userCom is null)
                throw new Exception("User not found in this company.");

            userCom.Rank = Domain.Enumerations.CompanyRank.Leader;
        }

        public async Task RankUserUpAsLeader(Guid userId)
        {
            var userCom = await _context.CompanyUsers
                .FirstOrDefaultAsync(uc => uc.UserId == userId);

            if (userCom is null)
                throw new Exception("User is not belong to this Company");

            userCom.Rank = Domain.Enumerations.CompanyRank.Leader;
        }

        public async Task<IEnumerable<CompanyUser>> GetAllMembersByCompanyIdAsync(Guid companyId)
        {
            return await _context.CompanyUsers
                .Where(cu => cu.CompanyId == companyId)
                .Include(cu => cu.User)
                .ToListAsync();
        }

        public async Task RemoveMemberAsync(Guid companyId, Guid userId)
        {
            await _context.CompanyUsers
                .Where(cu => cu.CompanyId == companyId && cu.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task RemoveAllMembersAsync(Guid companyId)
        {
            // Bulk-delete every membership row for this company in one DB round-trip
            await _context.CompanyUsers
                .Where(cu => cu.CompanyId == companyId)
                .ExecuteDeleteAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
