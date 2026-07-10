using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class ProjectTeamRepository : IProjectTeamRepository
    {
        private readonly AppDbContext _context;

        public ProjectTeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAlreadyAssignedAsync(int projectId, int teamId)
        {
            return await _context.ProjectTeams
                .AnyAsync(pt => pt.ProjectId == projectId && pt.TeamId == teamId);
        }

        public async Task AssignAsync(int projectId, int teamId)
        {
            var projectTeam = new ProjectTeam
            {
                ProjectId  = projectId,
                TeamId     = teamId,
                AssignedAt = DateTime.UtcNow
            };

            await _context.ProjectTeams.AddAsync(projectTeam);
        }

        public async Task UnassignAsync(int projectId, int teamId)
        {
            await _context.ProjectTeams
                .Where(pt => pt.ProjectId == projectId && pt.TeamId == teamId)
                .ExecuteDeleteAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
