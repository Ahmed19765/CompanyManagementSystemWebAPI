namespace CompanyManagementSystem.Application.Features.Commands.DeleteCompany
{
    public class DeleteCompanyResponse
    {
        public string Message { get; set; } = null!;

        // How many active tasks were unassigned — lets the owner know what needs follow-up
        public int UnassignedTaskCount { get; set; }

        // How many members were removed from the company
        public int RemovedMemberCount { get; set; }
    }
}
