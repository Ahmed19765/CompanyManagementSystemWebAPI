using CompanyManagementSystem.Domain.Enumerations;

namespace CompanyManagementSystem.Domain.Entities
{
    public class Project
    {
        public Guid ProjectId { get; set; } = Guid.NewGuid();
        public string? ProjectTitle { get; set; }
        public string? ProjectDescription { get; set; }
        public string? ProjectDocumentsUrl { get; set; }
        public decimal ProjectOfferedBudget { get; set; }
        public ProjectState? ProjectStatus { get; set; } = ProjectState.Pending;
        public DateTime? UploadedDate { get; set; } = DateTime.UtcNow;


        // Nullable — project survives if the customer user account is deleted
        public Guid? CustomerId { get; set; }
        public User? Customer { get; set; }

        // Teams assigned to work on this project (Many-to-Many)
        public ICollection<ProjectTeam> AssignedTeams { get; set; } = new List<ProjectTeam>();
        public ICollection<CompanyOffers> CompanyOffers { get; set; } = new List<CompanyOffers>();
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
