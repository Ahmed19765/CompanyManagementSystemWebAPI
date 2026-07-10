using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using CompanyManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CompanyManagementSystem.Infrastructure.Services
{
    // Adapts Identity's IPasswordHasher<User> to our existing IPasswordHasher interface.
    // The Application layer never changes — it still calls IPasswordHasher.HashPassword/VerifyPassword.
    // BCrypt.Net is no longer needed.
    public class PasswordHasher : IPasswordHasher
    {
        private readonly IPasswordHasher<User> _identityHasher;

        public PasswordHasher(IPasswordHasher<User> identityHasher)
        {
            _identityHasher = identityHasher;
        }

        public string HashPassword(string password)
        {
            // Passing null! for the user instance is fine — Identity's default PBKDF2 hasher
            // does not use the user object when hashing.
            return _identityHasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var result = _identityHasher.VerifyHashedPassword(null!, hashedPassword, password);
            return result != PasswordVerificationResult.Failed;
        }
    }
}
