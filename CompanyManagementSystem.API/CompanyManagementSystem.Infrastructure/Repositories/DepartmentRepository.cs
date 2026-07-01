using CompanyManagementSystem.Application.Interfaces.Repositories;
using CompanyManagementSystem.Domain.Entities;
using CompanyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompanyManagementSystem.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Company)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task<bool> ExistsByNameInCompanyAsync(string companyName, string departmentName)
        {
            var Company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyName == companyName);

            if (Company == null) 
            {
                throw new Exception("Company not found!");
            }

            return await _context.Departments
                .AnyAsync(d => d.CompanyId == Company.CompanyId && d.DepartmentName == departmentName);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
