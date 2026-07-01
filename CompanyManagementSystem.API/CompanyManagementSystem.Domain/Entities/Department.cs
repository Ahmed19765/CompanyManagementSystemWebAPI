namespace CompanyManagementSystem.Domain.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }


        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();

    }
}
