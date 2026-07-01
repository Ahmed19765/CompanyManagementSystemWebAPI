using CompanyManagementSystem.Domain.Entities;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(int id);
        Task AddAsync(Project project);
        Task SaveChangesAsync();
    }
}
