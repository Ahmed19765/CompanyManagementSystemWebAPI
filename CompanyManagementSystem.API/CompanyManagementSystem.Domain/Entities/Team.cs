namespace CompanyManagementSystem.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }

        public string? TeamName { get; set; }
        public string? TeamDescription { get; set; }

        // Team Leader — nullable so EF can ClientSetNull when the leader user is deleted
        public Guid? LeaderId { get; set; }
        public User? Leader { get; set; }

        // Department relation — nullable so a team can survive if its department is deleted
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Team members
        public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();

        // Tasks assigned to this team
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();

        // Projects this team is working on (Many-to-Many)
        public ICollection<ProjectTeam> AssignedProjects { get; set; } = new List<ProjectTeam>();
    }
}