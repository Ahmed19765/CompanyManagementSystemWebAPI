using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ICompanyUserRepository
    {
        Task<CompanyUser?> GetMembershipAsync(int companyId, Guid userId);
        Task<bool> IsMemberAsync(int companyId, Guid userId);
        Task AddAsync(CompanyUser companyUser);
        Task UpdateAsync(CompanyUser companyUser);
        Task SaveChangesAsync();
        Task RankUserUpAsLeader(string userName);
        Task RankUserUpAsLeader(Guid userId);

        // ── Query methods ──────────────────────────────────────────────────────────

        /// <summary>Returns all memberships (with User loaded) for a given company.</summary>
        Task<IEnumerable<CompanyUser>> GetAllMembersByCompanyIdAsync(int companyId);

        // ── Write methods ──────────────────────────────────────────────────────────

        /// <summary>Permanently removes the user's membership row from the company.</summary>
        Task RemoveMemberAsync(int companyId, Guid userId);

        /// <summary>Removes ALL member rows for the given company in one shot (used during company deletion).</summary>
        Task RemoveAllMembersAsync(int companyId);
    }
}
