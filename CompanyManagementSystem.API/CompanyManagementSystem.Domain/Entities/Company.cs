namespace CompanyManagementSystem.Domain.Entities
{
    public class Company
    {
        public Guid CompanyId { get; set; } = Guid.NewGuid();
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? JoinCode { get; set; } = Guid.NewGuid().ToString();

        // Soft-delete flag — company is never physically removed from the DB
        // so old project offers and history still reference it.
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // Owner relation (1-to-M)
        public Guid? OwnerId { get; set; }
        public User? Owner { get; set; }

        public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<CompanyOffers> CompanyOffers { get; set; } = new List<CompanyOffers>();
    }
}
