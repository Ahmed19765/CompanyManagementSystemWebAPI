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

        public async Task<CompanyUser?> GetMembershipAsync(int companyId, Guid userId)
        {
            return await _context.CompanyUsers
                .FirstOrDefaultAsync(cu => cu.CompanyId == companyId && cu.UserId == userId);
        }

        public async Task<bool> IsMemberAsync(int companyId, Guid userId)
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
            var userId = await _userRepository.GetUserIdFromUserName(userName);

            if (userId == Guid.Empty)
            {
                throw new Exception("User not exist!!");
            }
            await RankUserUpAsLeader(userId);
        }

        public async Task RankUserUpAsLeader(Guid userId)
        {

            var userCom = _context.CompanyUsers.FirstOrDefault(uc => uc.UserId == userId);

            if (userCom == null) 
            {
                throw new Exception("User is not belong to this Comapny");
            }

            userCom.Rank = Domain.Enumerations.CompanyRank.Leader;
        }

        public async Task<IEnumerable<CompanyUser>> GetAllMembersByCompanyIdAsync(int companyId)
        {
            return await _context.CompanyUsers
                .Where(cu => cu.CompanyId == companyId)
                .Include(cu => cu.User)
                .ToListAsync();
        }

        public async Task RemoveMemberAsync(int companyId, Guid userId)
        {
            await _context.CompanyUsers
                .Where(cu => cu.CompanyId == companyId && cu.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task RemoveAllMembersAsync(int companyId)
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
