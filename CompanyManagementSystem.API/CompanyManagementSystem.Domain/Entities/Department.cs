namespace CompanyManagementSystem.Domain.Entities
{
    public class Department
    {
        public Guid DepartmentId { get; set; } = Guid.NewGuid();
        public string? DepartmentName { get; set; }
        public string? DepartmentDescription { get; set; }

        // Nullable — a team can be unlinked from its department if the department is deleted,
        // and reassigned to a new one later.
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
