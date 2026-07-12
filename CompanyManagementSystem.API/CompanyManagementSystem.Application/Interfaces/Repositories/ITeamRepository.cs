using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(Guid id);
        Task AddAsync(Team team);
        Task<bool> ExistsByNameInDepartmentAsync(Guid departmentId, string teamName);
        Task SaveChangesAsync();

        // ── Query methods ──────────────────────────────────────────────────────────

        /// <summary>Returns all teams (with UserTeams + Leader) for a given department.</summary>
        Task<IEnumerable<Team>> GetAllByDepartmentIdAsync(Guid departmentId);

        /// <summary>Returns a team with its members (UserTeams → User) fully loaded.</summary>
        Task<Team?> GetWithMembersAsync(Guid teamId);

        /// <summary>Returns all teams led by the given user.</summary>
        Task<IEnumerable<Team>> GetAllByLeaderIdAsync(Guid leaderId);

        Task<Guid> CompanyIdFromTeamId(Guid TeamId);

        /// <summary>Deletes a team (nullifies Task.TeamId, deletes UserTeams, then removes the team).</summary>
        Task DeleteAsync(Guid teamId);

        // ── Write methods ──────────────────────────────────────────────────────────

        /// <summary>
        /// Removes the user from every team inside the given company:
        ///   - Deletes all UserTeam rows for this user in those teams.
        ///   - If the user leads any of those teams, sets Team.LeaderId = null.
        /// </summary>
        Task RemoveUserFromAllTeamsInCompanyAsync(Guid companyId, Guid userId);
    }
}
