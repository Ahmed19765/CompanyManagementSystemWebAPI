using System;
using System.Collections.Generic;
using System.Text;
using CompanyManagementSystem.Application.Common;
using MediatR;

namespace CompanyManagementSystem.Application.Features.Commands.Login
{
    public class LoginCommand : IRequest<Response<LoginResponse>>
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
