using System;

namespace CompanyManagementSystem.Application.Features.Commands.RefreshToken
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
