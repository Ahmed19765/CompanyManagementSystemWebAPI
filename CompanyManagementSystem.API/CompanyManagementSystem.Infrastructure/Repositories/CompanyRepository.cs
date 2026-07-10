using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Domain.Enumerations;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.CompanyId == id);
        }

        public async Task<Company?> GetByCompanyNameAsync(string companyName)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.CompanyName == companyName);
        }

        public async Task<int?> GetCompanyIdFromNameAsync(string companyName)
        {
            return await _context.Companies
                .Where(c => c.CompanyName == companyName)
                .Select(c => c.CompanyId)
                .FirstOrDefaultAsync();
        }

        public async Task<Company?> GetByJoinCodeAsync(string joinCode)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.JoinCode == joinCode);
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
        }

        public async Task<bool> IsCompanyExistWithSameName(string companyName)
        {
            return await _context.Companies
                .AnyAsync(c => c.CompanyName == companyName);
        }

        public async Task<IEnumerable<Company>> GetAllByOwnerIdAsync(Guid ownerId)
        {
            return await _context.Companies
                .Where(c => c.OwnerId == ownerId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Company?> GetWithDetailsAsync(int companyId)
        {
            return await _context.Companies
                .Include(c => c.CompanyUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Departments)
                    .ThenInclude(d => d.Teams)
                .Include(c => c.CompanyOffers)
                .FirstOrDefaultAsync(c => c.CompanyId == companyId && !c.IsDeleted);
        }

        public async Task SoftDeleteAsync(int companyId)
        {
            await _context.Companies
                .Where(c => c.CompanyId == companyId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.IsDeleted,  true)
                    .SetProperty(c => c.DeletedAt,  DateTime.UtcNow));
        }

        public async Task<bool> HasActiveOffersAsync(int companyId)
        {
            var blockingStatuses = new[] { OfferStatus.Pending, OfferStatus.Accepted };

            return await _context.CompanyOffers
                .AnyAsync(co => co.CompanyId == companyId
                             && blockingStatuses.Contains(co.Status));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
