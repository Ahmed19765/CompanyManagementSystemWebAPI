using CompanyManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Interfaces.Services.RegistrationAndLogin
{
    public interface IAccessJwtTokenGenerator
    {
        string GenerateAccessJwtToken(User user);
    }
}
