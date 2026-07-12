using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(Guid id);

        Task<Company?> GetByCompanyNameAsync(string CompanyName);

        Task<Guid?> GetCompanyIdFromNameAsync(string companyName);

        Task<Company?> GetByJoinCodeAsync(string joinCode);

        Task AddAsync(Company company);

        Task<bool> IsCompanyExistWithSameName(string companyName);

        // ── Query methods ──────────────────────────────────────────────────────────

        /// <summary>Returns all companies owned by the given user.</summary>
        Task<IEnumerable<Company>> GetAllByOwnerIdAsync(Guid ownerId);

        /// <summary>Returns a company with its Members (CompanyUsers + User), Departments, and Offers loaded.</summary>
        Task<Company?> GetWithDetailsAsync(Guid companyId);

        // ── Write methods ──────────────────────────────────────────────────────────

        /// <summary>
        /// Soft-deletes the company: sets IsDeleted = true and DeletedAt = now.
        /// The row stays in the DB so old project offers keep their CompanyId reference.
        /// </summary>
        Task SoftDeleteAsync(Guid companyId);

        /// <summary>
        /// Returns true if the company has any offers with status Pending or Accepted.
        /// Used to block deletion until the owner resolves active obligations.
        /// </summary>
        Task<bool> HasActiveOffersAsync(Guid companyId);

        Task SaveChangesAsync();
    }
}
