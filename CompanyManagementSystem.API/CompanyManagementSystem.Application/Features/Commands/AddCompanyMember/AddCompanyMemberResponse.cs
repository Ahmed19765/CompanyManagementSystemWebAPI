namespace CompanyManagementSystem.Application.Features.Commands.AddCompanyMember
{
    public class AddCompanyMemberResponse
    {
        public int CompanyId { get; set; }
        public string UserName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
