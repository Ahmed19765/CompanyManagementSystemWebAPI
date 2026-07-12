namespace CompanyManagementSystem.Application.Features.Commands.UpdateDepartment
{
    public class UpdateDepartmentResponse
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string? DepartmentDescription { get; set; }
        public string Message { get; set; } = null!;
    }
}
