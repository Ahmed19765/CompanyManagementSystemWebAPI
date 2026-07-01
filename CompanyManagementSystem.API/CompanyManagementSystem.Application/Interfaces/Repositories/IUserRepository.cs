using CompanyManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByUserNameAsync(string userName);

        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsByUserNameAsync(string userName);

        Task<bool> IsUserBannedAsync(string Email);

        Task<bool> IsUserEmailVerfiedAsync(string Email);

        Task<Guid> GetUserIdFromUserName(string userName);

        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);

        Task SaveChangesAsync();
    }
}
