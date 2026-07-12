using CompanyManagementSystem.Application.Common;
using CompanyManagementSystem.Domain.Enumerations;
using MediatR;
using System;

namespace CompanyManagementSystem.Application.Features.Commands.Register
{
    public class RegisterCommand : IRequest<Response<RegisterResponse>>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; }
    }
}
