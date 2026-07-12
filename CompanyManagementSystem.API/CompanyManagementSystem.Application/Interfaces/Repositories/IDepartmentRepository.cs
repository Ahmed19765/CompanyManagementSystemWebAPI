using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(Guid id);
        Task AddAsync(Department department);
        Task<bool> ExistsByNameInCompanyAsync(string companyName, string departmentName);
        Task<bool> ExistsByNameInCompanyAsync(Guid companyId, string departmentName, Guid? excludedDepartmentId = null);
        Task SaveChangesAsync();

        // ── Query methods ──────────────────────────────────────────────────────────

        /// <summary>Returns all departments (with their Teams) for a given company.</summary>
        Task<IEnumerable<Department>> GetAllByCompanyIdAsync(Guid companyId);

        // ── Write methods ──────────────────────────────────────────────────────────

        /// <summary>
        /// Hard-deletes all departments (and their teams via cascade) for a company.
        /// Called during company soft-delete after tasks and members are already cleared.
        /// </summary>
        Task DeleteAllByCompanyIdAsync(Guid companyId);

        /// <summary>
        /// Deletes a single department.
        /// All teams that belonged to it have their DepartmentId set to null (ClientSetNull)
        /// — they survive and can be reassigned to another department later.
        /// </summary>
        Task DeleteDepartmentAsync(Guid departmentId);
    }
}
