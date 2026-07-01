using CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin;
using System.Security.Cryptography;

namespace CompanyManagementSystem.Infrastructure.Services
{
    public class OtpGenerator : IOtpGenerator
    {
        public string GenerateOtp()
        {
            return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        }
    }
}
