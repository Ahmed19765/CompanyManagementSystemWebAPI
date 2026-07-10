using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class CompanyOffersRepository : ICompanyOffersRepository
    {
        private readonly AppDbContext _context;

        public CompanyOffersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyOffers?> GetByIdAsync(int companyId, int projectId)
        {
            return await _context.CompanyOffers
                .FirstOrDefaultAsync(co => co.CompanyId == companyId
                                        && co.ProjectId == projectId);
        }

        public async Task<bool> ExistsAsync(int companyId, int projectId)
        {
            return await _context.CompanyOffers
                .AnyAsync(co => co.CompanyId == companyId
                             && co.ProjectId == projectId);
        }

        public async Task<IEnumerable<CompanyOffers>> GetAllByProjectIdAsync(int projectId)
        {
            return await _context.CompanyOffers
                .Where(co => co.ProjectId == projectId)
                .Include(co => co.Company)
                .ToListAsync();
        }

        public async Task AcceptOfferAndRejectOthersAsync(int chosenCompanyId, int projectId)
        {
            // Set chosen offer → Accepted
            await _context.CompanyOffers
                .Where(co => co.ProjectId == projectId && co.CompanyId == chosenCompanyId)
                .ExecuteUpdateAsync(s => s.SetProperty(co => co.Status,
                    Domain.Enumerations.OfferStatus.Accepted));

            // Set all other offers on this project → Rejected
            await _context.CompanyOffers
                .Where(co => co.ProjectId == projectId && co.CompanyId != chosenCompanyId)
                .ExecuteUpdateAsync(s => s.SetProperty(co => co.Status,
                    Domain.Enumerations.OfferStatus.Rejected));
        }

        public async Task AddAsync(CompanyOffers offer)
        {
            await _context.CompanyOffers.AddAsync(offer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
