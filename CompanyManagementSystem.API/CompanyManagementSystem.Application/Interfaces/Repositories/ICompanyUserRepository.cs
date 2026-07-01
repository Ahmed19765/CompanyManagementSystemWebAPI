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


    }
}
