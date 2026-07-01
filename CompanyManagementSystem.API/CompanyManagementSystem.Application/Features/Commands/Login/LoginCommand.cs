using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
