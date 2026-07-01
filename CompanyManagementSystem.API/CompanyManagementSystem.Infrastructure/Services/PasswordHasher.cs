using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Security.Cryptography;

namespace CompanyManagementSystem.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 10; // Secuirty Increase with Increasing but lower proformance

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
