using CompanyManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string Token);

        Task CleanUsedToken(Guid id);
        Task CleanRevokedToken(Guid id);

        Task CleanAllUsedTokens();
        Task CleanAllRevokedTokens();

        Task DeleteAllUserRefreshTokens(Guid id);
        Task RevokeUserRefreshToken(Guid id);
        Task<bool> UseRefreshToken(Guid id, string Token);

        Task<string> CreateRefreshToken(Guid id, string Token);
        

    }
}
