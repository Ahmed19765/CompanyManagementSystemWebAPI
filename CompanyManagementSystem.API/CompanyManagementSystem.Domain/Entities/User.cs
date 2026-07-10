using CompanyManagementSystem.Domain.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace CompanyManagementSystem.Domain.Entities
{
    // Inheriting IdentityUser<Guid> provides:
    //   Id (replaces UserId), UserName, NormalizedUserName,
    //   Email, NormalizedEmail, EmailConfirmed (replaces IsEmailVerfied),
    //   PasswordHash (replaces Password), SecurityStamp, ConcurrencyStamp,
    //   PhoneNumber, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount
    public class User : IdentityUser<Guid>
    {
        // Custom domain fields — kept as-is
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserRole Role { get; set; }
        public bool IsBanned { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // All domain navigation properties — unchanged
        public ICollection<Company> OwnedCompanies { get; set; }
            = new List<Company>();

        public ICollection<UserTeam> UserTeams { get; set; }
            = new List<UserTeam>();

        public ICollection<CompanyUser> CompanyMemberships { get; set; }
            = new List<CompanyUser>();

        public ICollection<Team> LeadingTeams { get; set; }
            = new List<Team>();

        public ICollection<Project> OwnedProjects { get; set; }
            = new List<Project>();

        public ICollection<Tasks> AssignedByMe { get; set; }
            = new List<Tasks>();

        public ICollection<Tasks> AssignedToMe { get; set; }
            = new List<Tasks>();

        public ICollection<RefreshToken> RefreshToken { get; set; }
            = new List<RefreshToken>();
    }
}
