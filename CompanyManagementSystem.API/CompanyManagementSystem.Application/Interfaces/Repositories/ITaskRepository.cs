using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task AddAsync(Tasks task);
        Task SaveChangesAsync();

        // ── Query methods ──────────────────────────────────────────────────────────

        /// <summary>Returns a single task with Team → Department → Company loaded for membership checks.</summary>
        Task<Tasks?> GetByIdAsync(Guid taskId);

        /// <summary>Returns all tasks (with Details) assigned to a specific team.</summary>
        Task<IEnumerable<Tasks>> GetAllByTeamIdAsync(Guid teamId);

        /// <summary>Returns count of active tasks (Todo, InProgress, Pending) for a user.</summary>
        Task<int> CountActiveTasksByUserIdAsync(Guid userId);

        /// <summary>Returns count of active tasks across all teams in a company.</summary>
        Task<int> CountActiveTasksForCompanyAsync(Guid companyId);

        /// <summary>Returns all tasks assigned to a specific user (AssignedToId).</summary>
        Task<IEnumerable<Tasks>> GetAllAssignedToUserAsync(Guid userId);

        /// <summary>Returns all tasks belonging to a specific project.</summary>
        Task<IEnumerable<Tasks>> GetAllByProjectIdAsync(Guid projectId);

        /// <summary>Hard-deletes a task by its ID.</summary>
        Task DeleteAsync(Guid taskId);

        // ── Write methods ──────────────────────────────────────────────────────────

        /// <summary>
        /// Sets AssignedToId = null on every task assigned to this user
        /// that is still active (Todo, InProgress, Pending).
        /// Done and Failed tasks keep the user's Id — they record who did the work.
        /// </summary>
        Task NullifyPendingTaskAssignmentsAsync(Guid userId);

        /// <summary>
        /// Nullifies AssignedToId on all active tasks (Todo, InProgress, Pending)
        /// that belong to any team inside the given company.
        /// Done and Failed tasks are untouched — they carry the historical record.
        /// Called during company soft-delete so all in-flight work is unassigned at once.
        /// </summary>
        Task NullifyAllActiveTasksInCompanyAsync(Guid companyId);
    }
}
