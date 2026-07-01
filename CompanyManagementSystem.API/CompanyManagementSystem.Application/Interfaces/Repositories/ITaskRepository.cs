using CompanyManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task AddAsync(Tasks task);
        Task SaveChangesAsync();
    }
}
