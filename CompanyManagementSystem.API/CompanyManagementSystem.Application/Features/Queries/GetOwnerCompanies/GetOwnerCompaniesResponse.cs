namespace CompanyManagementSystem.Application.Features.Queries.GetOwnerCompanies
{
    public class GetOwnerCompaniesResponse
    {
        public IEnumerable<CompanyDto> Companies { get; set; } = new List<CompanyDto>();
    }

    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? CompanyDescription { get; set; }
        public string? JoinCode { get; set; }
        public int MemberCount { get; set; }
        public int DepartmentCount { get; set; }
    }
}
