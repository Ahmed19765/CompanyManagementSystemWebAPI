namespace CompanyManagementSystem.Application.Features.Queries.GetTeamMembers
{
    public class GetTeamMembersResponse
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public IEnumerable<TeamMemberDto> Members { get; set; } = new List<TeamMemberDto>();
    }

    public class TeamMemberDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? TeamRole { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
