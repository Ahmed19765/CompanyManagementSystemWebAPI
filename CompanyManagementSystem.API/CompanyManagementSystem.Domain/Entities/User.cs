using CompanyManagementSystem.Domain.Enumerations;

namespace CompanyManagementSystem.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }

        public bool IsEmailVerfied { get; set; } = false;
        public bool IsBanned { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Company relation (Owner)
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
