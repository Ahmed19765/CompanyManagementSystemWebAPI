using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Domain.Entities
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; } = Guid.NewGuid();

        public string Token { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        public bool IsUsed { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive =>
            !IsRevoked &&
            !IsUsed &&
            !IsExpired;
    }
}
