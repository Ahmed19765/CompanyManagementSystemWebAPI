namespace CompanyManagementSystem.Domain.Entities
{
    public class UserTeam
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        
        public string? TeamRole { get; set; } // Example: Developer, Tester, Scrum Master...
    }
}