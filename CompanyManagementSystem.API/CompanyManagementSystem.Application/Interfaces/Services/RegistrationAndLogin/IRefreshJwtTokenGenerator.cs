using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin
{
    public interface IRefreshJwtTokenGenerator
    {
        string GenerateRefreshToken();
    }
}
