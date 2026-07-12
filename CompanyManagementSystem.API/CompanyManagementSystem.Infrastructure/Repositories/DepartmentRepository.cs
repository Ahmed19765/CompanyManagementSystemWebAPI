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

        public async Task<Department?> GetByIdAsync(Guid id)
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

        public async Task<bool> ExistsByNameInCompanyAsync(Guid companyId, string departmentName, Guid? excludedDepartmentId = null)
        {
            return await _context.Departments
                .AnyAsync(d =>
                    d.CompanyId == companyId &&
                    d.DepartmentName == departmentName &&
                    (!excludedDepartmentId.HasValue || d.DepartmentId != excludedDepartmentId.Value));
        }

        public async Task<IEnumerable<Department>> GetAllByCompanyIdAsync(Guid companyId)
        {
            return await _context.Departments
                .Where(d => d.CompanyId == companyId)
                .Include(d => d.Teams)
                .ToListAsync();
        }

        public async Task DeleteAllByCompanyIdAsync(Guid companyId)
        {
            // Teams cascade-delete from Departments (ClientCascade in EF config),
            // and UserTeams cascade-delete from Teams.
            // We load them into memory first so EF's ClientCascade interceptors fire correctly.
            var departments = await _context.Departments
                .Where(d => d.CompanyId == companyId)
                .Include(d => d.Teams)
                    .ThenInclude(t => t.UserTeams)
                .ToListAsync();

            _context.Departments.RemoveRange(departments);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(Guid departmentId)
        {
            // Load with teams so EF's ClientSetNull interceptor
            // nulls Team.DepartmentId before the department row is removed.
            // Teams survive — they can be reassigned to another department later.
            var department = await _context.Departments
                .Include(d => d.Teams)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);

            if (department is null) return;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
