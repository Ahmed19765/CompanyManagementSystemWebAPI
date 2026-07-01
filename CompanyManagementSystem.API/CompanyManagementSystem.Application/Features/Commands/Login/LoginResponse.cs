using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Features.Commands.Login
{
    public class LoginResponse
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
