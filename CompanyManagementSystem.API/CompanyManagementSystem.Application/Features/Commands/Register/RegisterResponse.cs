using System;

namespace CompanyManagementSystem.Application.Features.Commands.Register
{
    public class RegisterResponse
    {
        public string UserName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
