namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyDepartments
{
    public class GetCompanyDepartmentsResponse
    {
        public int CompanyId { get; set; }
        public IEnumerable<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string? DepartmentDescription { get; set; }
        public int TeamCount { get; set; }
    }
}
