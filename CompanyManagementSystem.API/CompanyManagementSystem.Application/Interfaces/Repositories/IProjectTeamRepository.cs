using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IProjectTeamRepository
    {
        /// <summary>Returns true if this team is already assigned to this project.</summary>
        Task<bool> IsAlreadyAssignedAsync(int projectId, int teamId);

        /// <summary>Assigns a team to a project.</summary>
        Task AssignAsync(int projectId, int teamId);

        /// <summary>Removes the assignment of a team from a project.</summary>
        Task UnassignAsync(int projectId, int teamId);

        Task SaveChangesAsync();
    }
}
