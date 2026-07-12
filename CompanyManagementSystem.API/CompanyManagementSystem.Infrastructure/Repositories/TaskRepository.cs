using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tasks task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task<Tasks?> GetByIdAsync(Guid taskId)
        {
            return await _context.Tasks
                .Include(t => t.Team)
                    .ThenInclude(t => t.Department)
                        .ThenInclude(d => d.Company)
                .Include(t => t.Team)
                    .ThenInclude(t => t.UserTeams)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);
        }

        public async Task<IEnumerable<Tasks>> GetAllByTeamIdAsync(Guid teamId)
        {
            return await _context.Tasks
                .Where(t => t.TeamId == teamId)
                .Include(t => t.Details)
                .Include(t => t.AssignedTo)
                .Include(t => t.AssignedBy)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid taskId)
        {
            await _context.Tasks
                .Where(t => t.TaskId == taskId)
                .ExecuteDeleteAsync();
        }

        public async Task<int> CountActiveTasksByUserIdAsync(Guid userId)
        {
            var activeStates = new[]
            {
                Domain.Enumerations.TaskState.Todo,
                Domain.Enumerations.TaskState.InProgress,
                Domain.Enumerations.TaskState.Pending
            };

            return await _context.Tasks
                .CountAsync(t => t.AssignedToId == userId && activeStates.Contains(t.Status));
        }

        public async Task<int> CountActiveTasksForCompanyAsync(Guid companyId)
        {
            var activeStates = new[]
            {
                Domain.Enumerations.TaskState.Todo,
                Domain.Enumerations.TaskState.InProgress,
                Domain.Enumerations.TaskState.Pending
            };

            return await _context.Tasks
                .Where(t => t.Team != null
                         && t.Team.Department != null
                         && t.Team.Department.CompanyId == companyId
                         && activeStates.Contains(t.Status))
                .CountAsync();
        }

        public async Task<IEnumerable<Tasks>> GetAllAssignedToUserAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.AssignedToId == userId)
                .Include(t => t.Details)
                .Include(t => t.Project)
                .Include(t => t.Team)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tasks>> GetAllByProjectIdAsync(Guid projectId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Team)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task NullifyPendingTaskAssignmentsAsync(Guid userId)
        {
            // Only nullify tasks that are still active — todo, in-progress, pending.
            // Done and Failed tasks keep the userId so there's a record of who worked on them.
            var activeStates = new[]
            {
                Domain.Enumerations.TaskState.Todo,
                Domain.Enumerations.TaskState.InProgress,
                Domain.Enumerations.TaskState.Pending
            };

            await _context.Tasks
                .Where(t => t.AssignedToId == userId && activeStates.Contains(t.Status))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.AssignedToId, (Guid?)null));
        }

        public async Task NullifyAllActiveTasksInCompanyAsync(Guid companyId)
        {
            // Collect all team IDs that belong to this company's departments
            var teamIds = await _context.Teams
                .Where(t => t.Department.CompanyId == companyId)
                .Select(t => t.TeamId)
                .ToListAsync();

            if (!teamIds.Any()) return;

            var activeStates = new[]
            {
                Domain.Enumerations.TaskState.Todo,
                Domain.Enumerations.TaskState.InProgress,
                Domain.Enumerations.TaskState.Pending
            };

            // Nullify AssignedToId on all active tasks in those teams.
            // Done / Failed tasks keep their AssignedToId — historical record preserved.
            await _context.Tasks
                .Where(t => t.TeamId.HasValue
                         && teamIds.Contains(t.TeamId.Value)
                         && activeStates.Contains(t.Status))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.AssignedToId, (Guid?)null));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
