namespace CompanyManagementSystem.Application.Features.Queries.GetDepartmentTeams
{
    public class GetDepartmentTeamsResponse
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string? DepartmentDescription { get; set; }
        public IEnumerable<TeamSummaryDto> Teams { get; set; } = new List<TeamSummaryDto>();
    }

    public class TeamSummaryDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public string? TeamDescription { get; set; }
        public string? LeaderUserName { get; set; }
        public int MemberCount { get; set; }
    }
}
