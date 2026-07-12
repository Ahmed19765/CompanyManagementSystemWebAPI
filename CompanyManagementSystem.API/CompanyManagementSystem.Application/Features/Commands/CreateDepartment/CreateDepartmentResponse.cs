namespace CompanyManagementSystem.Application.Features.Commands.CreateDepartment
{
    public class CreateDepartmentResponse
    {
        public Guid DepartmentId { get; set; }
        public string Message { get; set; } = null!;
    }
}
