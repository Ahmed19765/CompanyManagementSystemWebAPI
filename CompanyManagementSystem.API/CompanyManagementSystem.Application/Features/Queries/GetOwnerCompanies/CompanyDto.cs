namespace CompanyManagementSystem.Application.Features.Queries.GetOwnerCompanies
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? CompanyDescription { get; set; }
        public string? JoinCode { get; set; }
        public int MemberCount { get; set; }
        public int DepartmentCount { get; set; }
    }
}
