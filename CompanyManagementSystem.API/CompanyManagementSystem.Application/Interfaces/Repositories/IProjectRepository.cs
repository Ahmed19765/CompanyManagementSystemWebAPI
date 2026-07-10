using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task AddAsync(Project project);
        Task SaveChangesAsync();

        // ── Query methods ──────────────────────────────────────────────────────────

        /// <summary>Returns all projects (with Tasks and AssignedTeams) owned by a customer.</summary>
        Task<IEnumerable<Project>> GetAllByCustomerIdAsync(Guid customerId);

        /// <summary>Returns a project with full details: Tasks, AssignedTeams, CompanyOffers.</summary>
        Task<Project?> GetWithDetailsAsync(int projectId);

        /// <summary>Returns all projects. Owners use this to browse and make offers.</summary>
        Task<IEnumerable<Project>> GetAllProjectsAsync();

        /// <summary>
        /// Returns all projects where the given company has an Accepted offer,
        /// with Tasks loaded so progress can be calculated.
        /// </summary>
        Task<IEnumerable<Project>> GetAcceptedProjectsByCompanyIdAsync(int companyId);

        /// <summary>Returns true if any offer on this project has status Accepted.</summary>
        Task<bool> HasAcceptedOfferAsync(int projectId);

        // ── Write methods ──────────────────────────────────────────────────────────

        /// <summary>Persists changes to an existing project.</summary>
        Task UpdateAsync(Project project);

        /// <summary>Hard-deletes a project row.</summary>
        Task DeleteAsync(int projectId);
    }
}
