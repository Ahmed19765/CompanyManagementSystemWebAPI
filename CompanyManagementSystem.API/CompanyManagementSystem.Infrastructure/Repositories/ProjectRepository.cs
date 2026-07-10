using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task<IEnumerable<Project>> GetAllByCustomerIdAsync(Guid customerId)
        {
            return await _context.Projects
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Project?> GetWithDetailsAsync(int projectId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedTo)
                .Include(p => p.AssignedTeams)
                    .ThenInclude(pt => pt.Team)
                .Include(p => p.CompanyOffers)
                    .ThenInclude(co => co.Company)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return _context.Projects.AsQueryable();
        }

        public async Task<IEnumerable<Project>> GetAcceptedProjectsByCompanyIdAsync(int companyId)
        {
            // Join through CompanyOffers — get all projects where this company's offer is Accepted
            return await _context.Projects
                .Where(p => p.CompanyOffers.Any(co =>
                    co.CompanyId == companyId &&
                    co.Status == Domain.Enumerations.OfferStatus.Accepted))
                .Include(p => p.Tasks)
                .Include(p => p.CompanyOffers.Where(co =>
                    co.CompanyId == companyId &&
                    co.Status == Domain.Enumerations.OfferStatus.Accepted))
                .ToListAsync();
        }

        public async Task<bool> HasAcceptedOfferAsync(int projectId)
        {
            return await _context.CompanyOffers
                .AnyAsync(co => co.ProjectId == projectId
                             && co.Status == Domain.Enumerations.OfferStatus.Accepted);
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int projectId)
        {
            await _context.Projects
                .Where(p => p.ProjectId == projectId)
                .ExecuteDeleteAsync();
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
