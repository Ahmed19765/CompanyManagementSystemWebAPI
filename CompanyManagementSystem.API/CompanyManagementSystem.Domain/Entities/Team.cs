namespace CompanyManagementSystem.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }

        public string? TeamName { get; set; }
        public string? TeamDescription { get; set; }

        // Team Leader
        public Guid LeaderId { get; set; }
        public User Leader { get; set; } = null!;

        // Department relation
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        // Team members
        public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();

        // Tasks assigned to this team
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();

        // Projects this team is working on (Many-to-Many)
        public ICollection<ProjectTeam> AssignedProjects { get; set; } = new List<ProjectTeam>();
    }
}