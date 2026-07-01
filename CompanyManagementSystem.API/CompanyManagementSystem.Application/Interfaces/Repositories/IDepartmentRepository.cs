using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(int id);
        Task AddAsync(Department department);
        Task<bool> ExistsByNameInCompanyAsync(string companyName, string departmentName);
        Task SaveChangesAsync();
    }
}
