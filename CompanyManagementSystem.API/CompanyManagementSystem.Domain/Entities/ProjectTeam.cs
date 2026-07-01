namespace CompanyManagementSystem.Domain.Entities
{
    // Join table: Project <---> Team (Many-to-Many)
    // One Project can be assigned to many Teams
    // One Team can work on many Projects
    public class ProjectTeam
    {
        // Composite Primary Key (configured in DbContext)
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
