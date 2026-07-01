using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(int id);

        Task<Company?> GetByCompanyNameAsync(string CompanyName);

        Task<int?> GetCompanyIdFromNameAsync(string companyName);

        Task<Company?> GetByJoinCodeAsync(string joinCode);

        Task AddAsync(Company company);

        Task<bool> IsCompanyExistWithSameName(string companyName);

        Task SaveChangesAsync();
    }
}
