namespace CompanyManagementSystem.Application.Features.Commands.AssignProjectToTeam
{
    public class AssignProjectToTeamResponse
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = null!;
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
