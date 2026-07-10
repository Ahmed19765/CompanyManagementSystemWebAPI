namespace CompanyManagementSystem.Application.Features.Commands.DeleteDepartment
{
    public class DeleteDepartmentResponse
    {
        public string Message { get; set; } = null!;

        // How many teams were unlinked — owner knows which teams need reassignment
        public int UnlinkedTeamCount { get; set; }
    }
}
