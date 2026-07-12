using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task AddAsync(Project project);
        Task SaveChangesAsync();

        // ── Query methods ──────────────────────────────────────────────────────────

        /// <summary>Returns all projects (with Tasks and AssignedTeams) owned by a customer.</summary>
        Task<IEnumerable<Project>> GetAllByCustomerIdAsync(Guid customerId);

        /// <summary>Returns a project with full details: Tasks, AssignedTeams, CompanyOffers.</summary>
        Task<Project?> GetWithDetailsAsync(Guid projectId);

        /// <summary>Returns all projects. Owners use this to browse and make offers.</summary>
        Task<IEnumerable<Project>> GetAllProjectsAsync();

        /// <summary>Returns all projects with Pending status, filtered in SQL.</summary>
        Task<IEnumerable<Project>> GetAllPendingProjectsAsync();

        /// <summary>
        /// Returns all projects where the given company has an Accepted offer,
        /// with Tasks loaded so progress can be calculated.
        /// </summary>
        Task<IEnumerable<Project>> GetAcceptedProjectsByCompanyIdAsync(Guid companyId);

        /// <summary>Returns true if any offer on this project has status Accepted.</summary>
        Task<bool> HasAcceptedOfferAsync(Guid projectId);

        // ── Write methods ──────────────────────────────────────────────────────────

        /// <summary>Persists changes to an existing project.</summary>
        Task UpdateAsync(Project project);

        /// <summary>Hard-deletes a project row.</summary>
        Task DeleteAsync(Guid projectId);
    }
}
