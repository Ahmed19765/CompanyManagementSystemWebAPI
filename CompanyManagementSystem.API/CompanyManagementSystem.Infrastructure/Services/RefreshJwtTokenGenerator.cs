using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using System;
using System.Security.Cryptography;

namespace CompanyManagementSystem.Infrastructure.Services
{
    public class RefreshJwtTokenGenerator : IRefreshJwtTokenGenerator
    {
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
