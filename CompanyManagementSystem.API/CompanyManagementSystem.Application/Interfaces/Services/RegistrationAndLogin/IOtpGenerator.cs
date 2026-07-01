using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin
{
    public interface IOtpGenerator
    {
        string GenerateOtp();
    }
}
