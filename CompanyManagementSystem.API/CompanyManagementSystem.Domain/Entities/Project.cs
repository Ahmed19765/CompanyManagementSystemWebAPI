namespace CompanyManagementSystem.Domain.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string? ProjectTitle { get; set; }
        public string? ProjectDescription { get; set; }
        public string? ProjectDocumentsUrl { get; set; }
        public decimal ProjectOfferedBudget { get; set; }
        public DateTime? UploadedDate { get; set; } = DateTime.UtcNow;


        public Guid CustomerId { get; set; }
        public User? Customer { get; set; }

        // Teams assigned to work on this project (Many-to-Many)
        public ICollection<ProjectTeam> AssignedTeams { get; set; } = new List<ProjectTeam>();
        public ICollection<CompanyOffers> CompanyOffers { get; set; } = new List<CompanyOffers>();
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
