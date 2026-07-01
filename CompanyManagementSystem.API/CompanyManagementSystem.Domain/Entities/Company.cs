namespace CompanyManagementSystem.Domain.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? JoinCode { get; set; } = Guid.NewGuid().ToString();

        // Owner relation (1-to-M)
        public Guid? OwnerId { get; set; }
        public User? Owner { get; set; }


        public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();

        public ICollection<Department> Departments { get; set; } = new List<Department>();

        public ICollection<CompanyOffers> CompanyOffers { get; set; } = new List<CompanyOffers>();
    }
}
