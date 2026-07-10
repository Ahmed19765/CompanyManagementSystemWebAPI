using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;


        public TeamRepository(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.Department)
                .ThenInclude(d => d.Company)
                .Include(t => t.UserTeams)
                .FirstOrDefaultAsync(t => t.TeamId == id);
        }

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        public async Task<bool> ExistsByNameInDepartmentAsync(int departmentId, string teamName)
        {
            return await _context.Teams
                .AnyAsync(t => t.DepartmentId == departmentId && t.TeamName == teamName);
        }

        public async Task<IEnumerable<Team>> GetAllByDepartmentIdAsync(int departmentId)
        {
            return await _context.Teams
                .Where(t => t.DepartmentId == departmentId)
                .Include(t => t.Leader)
                .Include(t => t.UserTeams)
                    .ThenInclude(ut => ut.User)
                .ToListAsync();
        }

        public async Task<Team?> GetWithMembersAsync(int teamId)
        {
            return await _context.Teams
                .Include(t => t.Leader)
                .Include(t => t.UserTeams)
                    .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);
        }

        public async Task<IEnumerable<Team>> GetAllByLeaderIdAsync(Guid leaderId)
        {
            return await _context.Teams
                .Where(t => t.LeaderId == leaderId)
                .Include(t => t.UserTeams)
                    .ThenInclude(ut => ut.User)
                .Include(t => t.Department)
                .ToListAsync();
        }

        public async Task<int> CompanyIdFromTeamId(int TeamId)
        {
            var team = await _context.Teams
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.TeamId == TeamId);
            if (team == null)
            {
                throw new Exception("Team not found.");
            }
            return team.Department?.CompanyId ?? 0;
        }

        public async Task RemoveUserFromAllTeamsInCompanyAsync(int companyId, Guid userId)
        {
            // Get all team IDs that belong to this company
            var teamIdsInCompany = await _context.Teams
                .Where(t => t.Department.CompanyId == companyId)
                .Select(t => t.TeamId)
                .ToListAsync();

            if (!teamIdsInCompany.Any()) return;

            // 1. Delete all UserTeam rows for this user in those teams
            await _context.UserTeams
                .Where(ut => ut.UserId == userId && teamIdsInCompany.Contains(ut.TeamId))
                .ExecuteDeleteAsync();

            // 2. Null out LeaderId on any team this user was leading
            await _context.Teams
                .Where(t => teamIdsInCompany.Contains(t.TeamId) && t.LeaderId == userId)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.LeaderId, (Guid?)null));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
