using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IProjectTeamRepository
    {
        /// <summary>Returns true if this team is already assigned to this project.</summary>
        Task<bool> IsAlreadyAssignedAsync(Guid projectId, Guid teamId);

        /// <summary>Assigns a team to a project.</summary>
        Task AssignAsync(Guid projectId, Guid teamId);

        /// <summary>Removes the assignment of a team from a project.</summary>
        Task UnassignAsync(Guid projectId, Guid teamId);

        Task SaveChangesAsync();
    }
}
