using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(int id);
        Task AddAsync(Team team);
        Task<bool> ExistsByNameInDepartmentAsync(int departmentId, string teamName);
        Task SaveChangesAsync();
    }
}
