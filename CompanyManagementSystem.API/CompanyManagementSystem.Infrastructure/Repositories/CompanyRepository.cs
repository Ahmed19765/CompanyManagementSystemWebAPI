using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
