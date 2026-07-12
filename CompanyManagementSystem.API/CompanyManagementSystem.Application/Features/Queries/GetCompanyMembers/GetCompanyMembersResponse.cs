namespace CompanyManagementSystem.Application.Features.Queries.GetCompanyMembers
{
    public class GetCompanyMembersResponse
    {
        public Guid CompanyId { get; set; }
        public IEnumerable<CompanyMemberDto> Members { get; set; } = new List<CompanyMemberDto>();
    }

    public class CompanyMemberDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Rank { get; set; } = null!;
        public DateTime JoinedAt { get; set; }
    }
}
