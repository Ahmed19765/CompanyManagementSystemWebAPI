namespace CompanyManagementSystem.Application.Features.Commands.RemoveCompanyMember
{
    public class RemoveCompanyMemberResponse
    {
        public string Message { get; set; } = null!;

        // How many active tasks were unassigned so the owner knows what needs reassignment
        public int UnassignedTaskCount { get; set; }
    }
}
