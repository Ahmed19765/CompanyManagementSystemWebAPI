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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
